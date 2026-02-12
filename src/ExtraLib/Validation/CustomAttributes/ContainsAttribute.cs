namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Атрибут для проверки содержания в строке указанной подстроки
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ContainsAttribute : ValidationAttributeTemplate<string>
{
    private readonly string _stringForCheckContains;

    /// <summary>
    /// Создание атрибута для проверки содержания в строке указанной подстроки
    /// </summary>
    /// <param name="stringForCheckContains">Подстрока для проверки содержания</param>
    /// <param name="errorMessage">Сведенья об ошибке</param>
    public ContainsAttribute(string stringForCheckContains, string? errorMessage = null) : base(errorMessage, stringForCheckContains) =>
        _stringForCheckContains = stringForCheckContains;

    private protected override (bool IsValid, string? Error) IsValidAttributeParameters()
    {
        if (string.IsNullOrEmpty(_stringForCheckContains))
            return (false, "The string whose contents are being checked cannot be empty");
        else
            return (true, null);
    }

    private protected override (bool IsValid, string? Error) IsValidTargetValue(string validable)
    {
        if (validable.Contains(_stringForCheckContains))
            return (true, null);
        else
            return (false, $"The string should contain the \"{_stringForCheckContains}\"");
    }
}