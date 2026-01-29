namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Статический класс с методами расширения для строк
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Метод сокращающий строку до указанной длины
    /// </summary>
    /// <param name="input">Строковая переменная</param>
    /// <param name="length">Длина выходной строки</param>
    /// <returns>Возвращает кортеж со значениями "Message" — Строка, сокращённая до указанной длины; и "Remainder" — Остаточная длина (при наличии)</returns>
    public static (string Message, int Remainder) Reduce(this string input, int length)
    {
        if (length < 3)
            return (string.Empty, 0);

        int charsLength = length - input.Length;

        if (charsLength > 0)
            return (input, charsLength);
        else
            return (input[0..(input.Length + charsLength - 3)] + "...", 0);
    }

    /// <summary>
    /// Заменяет старое значение на новое, пока в тексте содержится старое значение (удобно для удаления повторяющихся символов)
    /// </summary>
    /// <param name="text">Исходный текст, в котором требуется заменить символы</param>
    /// <param name="oldValue">Старое значение</param>
    /// <param name="newValue">Новое значение</param>
    /// <returns>Текст со всеми осуществлёнными заменами</returns>
    public static string ReplaceWhileContain(this string text, string oldValue, string newValue)
    {
        string pre = oldValue;
        bool isReplacedAll = false;

        while (!isReplacedAll)
        {
            text = text.Replace(oldValue, newValue);
            isReplacedAll = text == pre;
            pre = text;
        }

        return text;
    }
}