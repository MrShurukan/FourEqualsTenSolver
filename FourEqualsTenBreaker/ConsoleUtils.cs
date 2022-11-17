namespace FourEqualsTenBreaker;

internal class ConsoleUtils
{
    public static void LocaleWrite(Text text)
        => Console.Write(Localization.GetText(text));

    public static void LocaleWriteLine(Text text)
    {
        Console.WriteLine(Localization.GetText(text));
    }

    public static void WriteHello()
    {
        Console.ForegroundColor = ConsoleColor.White;
        LocaleWriteLine(Text.WelcomeMessage);
        Console.WriteLine();
    }

    public static (int[] numbers, char[] availableOperators) GetInitialValues()
    {
        Console.ResetColor();
        LocaleWrite(Text.EnterFourNumbers);
        
        var numberStr = Console.ReadLine();
        if (numberStr!.Length < 4) ExitWithError(Text.InputIsLessThanFourNumbers);
        
        var numbers = numberStr.ToCharArray()
            .Select(numChar =>
            {
                if (!char.IsDigit(numChar))
                    ExitWithError(Text.CantParseInputToIntegers);

                return ConvertCharToInt(numChar);
            }).ToArray();
        
        LocaleWrite(Text.EnterOperators);
        
        var operatorsStr = Console.ReadLine();
        if (string.IsNullOrEmpty(operatorsStr)) operatorsStr = "(+-*/)";

        var operators = operatorsStr.ToCharArray();

        return (numbers, operators);
    }

    public static void ExitWithError(Text errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        LocaleWriteLine(errorMessage);

        Console.ResetColor();
        Environment.Exit(0);
    }

    // The classic approach :)
    // Input has to be a valid digit
    private static int ConvertCharToInt(char c)
    {
        return c - '0';
    }
}