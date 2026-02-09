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
        string pre = text;
        bool isReplacedAll = false;

        while (!isReplacedAll)
        {
            text = text.Replace(oldValue, newValue);
            isReplacedAll = text == pre;
            pre = text;
        }

        return text;
    }

    /// <summary>
    /// Попытка форматирования строки
    /// </summary>
    /// <param name="text">Строка для форматирования</param>
    /// <param name="formatted">Отформатированная строка в случае успеха, или пустая строка в случае неуспеха</param>
    /// <param name="valuesToInternalization">Переменные для интернирования</param>
    /// <returns>Логическое значение, отражающее получилось ли форматировать указанную строку</returns>
    public static bool TryFormat(this string text, out string formatted, params object[] valuesToInternalization)
    {
        try
        {
            formatted = string.Format(text, valuesToInternalization);

            if (IsContainsAllInternedValue(formatted, valuesToInternalization))
                return true;
            else
                return ClearOutStringAndReturnFalse(out formatted);
        }
        catch
        {
            return ClearOutStringAndReturnFalse(out formatted);
        }

        static bool IsContainsAllInternedValue(string formattedString, object[] valuesToInternalization)
        {
            foreach (var value in valuesToInternalization)
                if (!formattedString.Contains(value.ToString()!))
                    return false;

            return true;
        }

        static bool ClearOutStringAndReturnFalse(out string formatted)
        {
            formatted = string.Empty;
            return false;
        }
    }

    /// <summary>
    /// Проверка валидности строки на предмет интернирования
    /// </summary>
    /// <param name="text">Строка для проверки</param>
    /// <param name="internedCount">Количество переменных для интернирования</param>
    /// <returns>Результат, отражающий поддерживает ли строка интернирование, указанного количества переменных</returns>
    public static (bool IsValid, string? Error) IsValidForInternalization(this string text, int internedCount)
    {
        if (!string.IsNullOrEmpty(text) && text.TryFormat(out _, CreateRandomValues(internedCount)))
            return (true, null);
        else
            return (false, $"The specified string does not support internalization ({internedCount})");

        static object[] CreateRandomValues(int cnt)
        {
            var values = new object[cnt];

            for (int i = 0; i < cnt; i++)
                values[i] = int.RandomValue;

            return values;
        }
    }
}