namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Атрибут для проверки того, поддерживает ли строка интернирование, указанного количества переменных
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InternalizationSupportedAttribute : ValidationAttributeTemplate<string>
{
    private int _internalizationableCount;

    /// <summary>
    /// Создание атрибута для проверки того, поддерживает ли строка интернирование
    /// </summary>
    /// <param name="internalizationableCount">Количество переменных для интернирование</param>
    /// <param name="errorMessage">Сведенья об ошибке</param>
    /// <exception cref="FormatException">Невалидное число переменных (меньшее 1)</exception>
    public InternalizationSupportedAttribute(int internalizationableCount, string? errorMessage = null) : base(errorMessage, internalizationableCount)
    {
        if (internalizationableCount < 1)
            throw new FormatException("The number of variables to be internalized cannot be less than 1");

        _internalizationableCount = internalizationableCount;
    }

    private protected override (bool IsValid, string? Error) IsValid(string validable)
    {
        var validationResult = validable.IsValidForInternalization(_internalizationableCount);
        if (validationResult.IsValid)
            return (true, null);
        else
            return (false, validationResult.Error);
    }
}