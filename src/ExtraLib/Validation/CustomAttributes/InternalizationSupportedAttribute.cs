namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Атрибут для проверки того, поддерживает ли строка интернирование, указанного количества переменных
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InternalizationSupportedAttribute : ValidationAttributeTemplate<string>
{
    private readonly int _internalizationableCount;

    /// <summary>
    /// Создание атрибута для проверки того, поддерживает ли строка интернирование
    /// </summary>
    /// <param name="internalizationableCount">Количество переменных для интернирование</param>
    /// <param name="errorMessage">Сведенья об ошибке</param>
    /// <exception cref="FormatException">Невалидное число переменных (меньшее 1)</exception>
    public InternalizationSupportedAttribute(int internalizationableCount, string? errorMessage = null) : base(errorMessage, internalizationableCount) =>
        _internalizationableCount = internalizationableCount;

    private protected override (bool IsValid, string? Error) IsValidAttributeParameters()
    {
        if (_internalizationableCount < 1)
            return (false, "The number of variables to be internalized cannot be less than 1");
        else
            return (true, null);
    }

    private protected override (bool IsValid, string? Error) IsValidTargetValue(string validable)
    {
        var validationResult = validable.IsValidForInternalization(_internalizationableCount);
        if (validationResult.IsValid)
            return (true, null);
        else
            return (false, validationResult.Error);
    }
}