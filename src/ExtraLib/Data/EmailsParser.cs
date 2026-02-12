namespace san40_u5an40.ExtraLib.Data;

/// <summary>
/// Класс для работы с текстом, содержащим email-адреса
/// </summary>
public static class EmailsParser
{
    /// <summary>
    /// Регулярное выражение, для поиска в тексте email-адресов
    /// </summary>
    public const string EmailRegex = @"\b(?:\w|\d|_|\-|\.)+?@\w+?\.\w+?\b";

    /// <summary>
    /// Меняет все email-адреса из текста на указанное значение
    /// </summary>
    /// <param name="text">Исходный текст для поиска и замены email-адресов</param>
    /// <param name="replacesStr">Значение, на которое будет заменён найденный email</param>
    /// <returns>Текст с заменёнными адресами</returns>
    public static string Replace(string text, string? replacesStr)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        replacesStr ??= string.Empty;

        return Regex
            .Replace(text,
                     EmailRegex,
                     replacesStr,
                     RegexOptions.Compiled,
                     TimeSpan.FromMilliseconds(100));
    }

    /// <summary>
    /// Получение коллекции email-адресов, хранящихся в тексте
    /// </summary>
    /// <param name="text">Исходный текст для поиска email-адресов</param>
    /// <returns>Коллекция найденных email-адресов</returns>
    public static List<string> Parse(string text)
    {
        if (string.IsNullOrEmpty(text))
            return [];
        
        return Regex
            .Matches(text,
                     EmailRegex,
                     RegexOptions.Compiled,
                     TimeSpan.FromMilliseconds(100))
            .Select(p => p.Value)
            .ToList();
    }

    /// <summary>
    /// Проверка email-адреса на валидность
    /// </summary>
    /// <param name="email">Проверяемый адрес</param>
    /// <returns>Валидность адреса</returns>
    public static bool IsValid(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return Regex
            .IsMatch(email, 
                     '^' + EmailRegex + '$',
                     RegexOptions.Compiled,
                     TimeSpan.FromMilliseconds(100));
    }
}