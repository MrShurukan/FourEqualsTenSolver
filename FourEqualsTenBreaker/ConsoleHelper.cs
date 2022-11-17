namespace FourEqualsTenBreaker;

internal class ConsoleHelper
{
    public static void WriteHello()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Welcome to 4=10 solver\n");
    }

    public static (int[] numbers, char[] availableOperators) GetInitialValues()
    {
        Console.ResetColor();
        Console.Write("Введите 4 числа (без пробелов): ");
        var numberStr = Console.ReadLine();
        if (numberStr!.Length < 4) ExitWithError("Чисел было меньше 4");
        var numbers = numberStr.ToCharArray().Select(x => x.ToString())
            .Select(numChar =>
            {
                if (!int.TryParse(numChar, out var numParsed))
                    ExitWithError("Не удалось преобразовать числа в integer");

                return numParsed;
            }).ToArray();
        
        Console.Write("Введите операторы [нажмите Enter для (+-*/)]: ");
        var operatorsStr = Console.ReadLine();
        if (string.IsNullOrEmpty(operatorsStr)) operatorsStr = "(+-*/)";

        var operators = operatorsStr.ToCharArray();

        return (numbers, operators);
    }

    public static void ExitWithError(string errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(errorMessage);

        Environment.Exit(0);
    }
}