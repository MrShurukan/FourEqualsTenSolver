using static FourEqualsTenBreaker.Text;

namespace FourEqualsTenBreaker;

public enum Languages
{
    English,
    Russian
}

public enum Text
{
    WelcomeMessage,
    EnterFourNumbers,
    InputIsLessThanFourNumbers,
    CantParseInputToIntegers,
    EnterOperators,
    CouldntCreateFileDueToError,
    OutputPathWasNotProvided,
    Permutation,
    NoSolutionsFound,
    FoundSolutions
}

public static class Localization
{
    public static Languages CurrentLanguage = Languages.English;

    public static string GetText(Text textType)
    {
        return CurrentLanguage switch
        {
            Languages.English => EnglishLocale[textType],
            Languages.Russian => RussianLocale[textType],
            _ => throw new ArgumentOutOfRangeException(nameof(textType))
        };
    }
    
    private static readonly Dictionary<Text, string> EnglishLocale = new()
    {
        { WelcomeMessage, "Welcome to 4=10 solver" },
        { EnterFourNumbers, "Enter four numbers (no spaces): " },
        { InputIsLessThanFourNumbers, "Input is less than four numbers" },
        { CantParseInputToIntegers, "Can't parse input to integers" },
        { EnterOperators, "Enter operators [press Enter for (+-*/)]: " },
        { CouldntCreateFileDueToError, "Couldn't create file due to error" },
        { OutputPathWasNotProvided, "No file path was provided" },
        { Permutation, "Permutation" },
        { NoSolutionsFound, "No solutions found" },
        { FoundSolutions, "Solutions found" }
    };

    private static readonly Dictionary<Text, string> RussianLocale = new()
    {
        { WelcomeMessage, "Добро пожаловать в 4=10 помощник" },
        { EnterFourNumbers, "Введите 4 числа (без пробелов): " },
        { InputIsLessThanFourNumbers, "Чисел было меньше 4" },
        { CantParseInputToIntegers, "Не могу преобразовать входные данные в целые числа" },
        { EnterOperators, "Введите операторы [нажмите Enter для (+-*/)]: " },
        { CouldntCreateFileDueToError, "Не удалось создать файл из-за ошибки" },
        { OutputPathWasNotProvided, "Путь к выходному файлу не был предоставлен" },
        { Permutation, "Пермутация" },
        { NoSolutionsFound, "Не найдено решений" },
        { FoundSolutions, "Решения найдены" }
    };
}