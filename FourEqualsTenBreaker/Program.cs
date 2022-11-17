using System.Text;

namespace FourEqualsTenBreaker;

internal static class Program 
{
    public static void Main()
    {
        ConsoleHelper.WriteHello();

        var (numbers, availableOperators) = ConsoleHelper.GetInitialValues();

        var operatorsInfo = new OperatorsInfo(
            PlusAvailable: availableOperators.Contains('+'),
            MinusAvailable: availableOperators.Contains('-'),
            DivisionAvailable: availableOperators.Contains('/'),
            MultiplicationAvailable: availableOperators.Contains('*'),
            BracketsAvailable: availableOperators.Contains('(') && availableOperators.Contains(')'));
        
        CalculateAllPossibleSolutions(numbers, operatorsInfo);
    }

    private static void CalculateAllPossibleSolutions(int[] numbers, OperatorsInfo operatorsInfo)
    {
        var permutations = PermutationSolver.Permute(numbers);
        var validAnswers = new List<string>();

        for (var permutationIndex = 0; permutationIndex < permutations.Length; permutationIndex++)
        {
            var permutation = permutations[permutationIndex];
            
            Console.ResetColor();
            Console.Write($"Пермутация №{permutationIndex+1} ({string.Join("", permutation)})... ");

            var algebraicNotations = new List<string>();
            GetAlgebraicNotationsOfPermutation(permutation, operatorsInfo, ref algebraicNotations);

            var polishNotations = algebraicNotations.Select(GetPolishNotationFromAlgebraic).ToList();

            var permutationValidAnswers = new List<string>();
            for (var polishNotationIndex = 0; polishNotationIndex < polishNotations.Count; polishNotationIndex++)
            {
                var polishNotation = polishNotations[polishNotationIndex];
                
                double answer = CalculatePolishNotation(polishNotation);

                if (Math.Abs(answer - 10) < 0.0001) 
                    permutationValidAnswers.Add(algebraicNotations[polishNotationIndex]);
            }
            
            if (permutationValidAnswers.Count == 0)
                Console.WriteLine("не найдено решений");
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"найдено {permutationValidAnswers.Count} решений!");
            }

            validAnswers.AddRange(permutationValidAnswers);
        }
        
        Console.WriteLine(string.Join("\n", validAnswers));
    }

    private static double CalculatePolishNotation(string polishNotation)
    {
        var stack = new Stack<double>();
        foreach (var c in polishNotation)
        {
            if (char.IsDigit(c))
            {
                stack.Push(double.Parse(c.ToString()));
                continue;
            }

            double secondOperand = stack.Pop(), firstOperand = stack.Pop();

            var result = c switch
            {
                '+' => firstOperand + secondOperand,
                '-' => firstOperand - secondOperand,
                '/' => firstOperand / secondOperand,
                '*' => firstOperand * secondOperand,
                _ => 0.0
            };

            stack.Push(result);
        }

        return stack.Pop();
    }

    enum OperationPriorities
    {
        Lowest,
        Low,
        Medium,
        High
    }

    private static readonly Dictionary<char, OperationPriorities> PrioritiesMap = new()
    {
        {'*', OperationPriorities.High},
        {'/', OperationPriorities.High},
        {'+', OperationPriorities.Low},
        {'-', OperationPriorities.Low},
        {'(', OperationPriorities.Lowest},
        {')', OperationPriorities.Lowest},
    };

    private static string GetPolishNotationFromAlgebraic(string algebraicNotation)
    {
        var stack = new Stack<char>();
        var resultStringBuilder = new StringBuilder();

        foreach (var c in algebraicNotation)
        {
            if (char.IsDigit(c))
            {
                resultStringBuilder.Append(c);
                continue;
            }
            
            switch (c)
            {
                case '(':
                    stack.Push(c);
                    break;
                case ')':
                    while (stack.Count != 0 && stack.Peek() != '(')
                        resultStringBuilder.Append(stack.Pop());

                    stack.Pop();
                    
                    break;
                default:
                    if (stack.Count != 0)
                    {
                        while (stack.Count != 0 && PrioritiesMap[stack.Peek()] > PrioritiesMap[c])
                            resultStringBuilder.Append(stack.Pop());
                    }
                    
                    stack.Push(c);
                    break;
            }
        }

        while (stack.Count != 0)
            resultStringBuilder.Append(stack.Pop());

        return resultStringBuilder.ToString();
    }

    private static void GetAlgebraicNotationsOfPermutation(int[] numbers, OperatorsInfo operatorsInfo, 
        ref List<string> algebraicNotations, char[]? operators = null)
    {
        operators ??= Array.Empty<char>();
        
        // Шаг 2)
        // Мы закончили заполнять операторы, время вкинуть скобки, если это возможно!
        if (operators.Length == numbers.Length - 1)
        {
            var possibleVariantsWithBrackets = GetPossibleVariantsWithBrackets(numbers, operators, operatorsInfo.BracketsAvailable);
            algebraicNotations.AddRange(possibleVariantsWithBrackets);
            
            return;
        }
        
        // Шаг 1)
        // Заполнить все возможные места для операторов операторами +-/*. Скобки пока не учитывать
        if (operatorsInfo.PlusAvailable) 
            GetAlgebraicNotationsOfPermutation(numbers, operatorsInfo, ref algebraicNotations, operators.Append('+').ToArray());
        if (operatorsInfo.MinusAvailable) 
            GetAlgebraicNotationsOfPermutation(numbers, operatorsInfo, ref algebraicNotations, operators.Append('-').ToArray());
        if (operatorsInfo.DivisionAvailable) 
            GetAlgebraicNotationsOfPermutation(numbers, operatorsInfo, ref algebraicNotations, operators.Append('/').ToArray());
        if (operatorsInfo.MultiplicationAvailable) 
            GetAlgebraicNotationsOfPermutation(numbers, operatorsInfo, ref algebraicNotations, operators.Append('*').ToArray());
    }

    private static List<string> GetPossibleVariantsWithBrackets(int[] numbers, char[] operators, bool bracketsAvailable)
    {
        var results = new List<string>();
        if (bracketsAvailable)
        {
            for (var startIndex = 0; startIndex < numbers.Length; startIndex++)
            {
                for (var endIndex = startIndex + 1; endIndex < numbers.Length; endIndex++)
                {
                    results.Add(FillWithOneBracketPair(numbers, operators, startIndex, endIndex));
                }
            }
        }
        else
        {
            results.Add(FillWithOneBracketPair(numbers, operators, -1, -1));
        }

        return results;
    }

    private static string FillWithOneBracketPair(int[] numbers, char[] operators, int startIndex, int endIndex)
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < numbers.Length; i++)
        {
            if (startIndex == i)
                stringBuilder.Append('(');

            stringBuilder.Append(numbers[i]);
            
            if (endIndex == i)
                stringBuilder.Append(')');
            
            if (i < operators.Length)
                stringBuilder.Append(operators[i]);
        }

        return stringBuilder.ToString();
    }
}

internal record OperatorsInfo(bool PlusAvailable, bool MinusAvailable, bool DivisionAvailable,
    bool MultiplicationAvailable, bool BracketsAvailable);