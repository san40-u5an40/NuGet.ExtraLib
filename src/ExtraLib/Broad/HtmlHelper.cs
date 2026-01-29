namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Вспомогательный класс, для работы с Html-текстом
/// </summary>
public class HtmlHelper
{
    /// <summary>
    /// Регулярное выражение для поиска тегов в тексте
    /// </summary>
    public const string TagRegex = @"<\s?/?.+?>";

    /// <summary>
    /// Очищает строку от тегов
    /// </summary>
    /// <param name="input">Исходный текст для очистки от тегов</param>
    /// <returns>Очищенный текст</returns>
    public static string TagsClear(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return Regex
            .Replace(input,
                     TagRegex,
                     string.Empty,
                     RegexOptions.Compiled | RegexOptions.Singleline,
                     TimeSpan.FromMilliseconds(100))
            .ReplaceWhileContain("  ", " ")
            .ReplaceWhileContain("\n ", "\n")
            .ReplaceWhileContain("\r\n\r\n", "\r\n")
            .ReplaceWhileContain("\n\n", "\n\n")
            .Trim('\r', '\n', ' ');
    }
}