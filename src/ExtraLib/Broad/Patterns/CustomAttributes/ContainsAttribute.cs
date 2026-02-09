namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Атрибут для проверки содержания в строке указанной подстроки
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ContainsAttribute : ValidationAttributeTemplate<string>
{
    private string _stringForCheckContains;

    /// <summary>
    /// Создание атрибута для проверки содержания в строке указанной подстроки
    /// </summary>
    /// <param name="stringForCheckContains">Подстрока для проверки содержания</param>
    /// <param name="errorMessage">Сведенья об ошибке</param>
    public ContainsAttribute(string stringForCheckContains, string? errorMessage = null) : base(errorMessage, stringForCheckContains) =>
        _stringForCheckContains = stringForCheckContains;

    private protected override (bool IsValid, string? Error) IsValid(string validable)
    {
        if (validable.Contains(_stringForCheckContains))
            return (true, null);
        else
            return (false, $"The string should contain the \"{_stringForCheckContains}\"");
    }
}