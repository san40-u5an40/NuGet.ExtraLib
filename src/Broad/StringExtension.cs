namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Статический класс с методами расширения для строк
/// </summary>
public static class StringExtension
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
}