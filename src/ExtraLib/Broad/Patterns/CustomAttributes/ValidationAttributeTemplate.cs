namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Шаблон для создания атрибутов валидации
/// </summary>
/// <typeparam name="T">Тип проверяемого свойства или класса</typeparam>
public abstract class ValidationAttributeTemplate<T>(string? errorMessage, params object[] parameters) : ValidationAttribute
    where T : class
{
    private protected abstract (bool IsValid, string? Error) IsValid(T validable);

    /// <summary>
    /// Проверка валидности указанного значения при помощи атрибута
    /// </summary>
    /// <param name="value">Проверяемое значение</param>
    /// <returns>Логическая переменная, показывающая валидность проверяемого значения</returns>
    public sealed override bool IsValid(object? value)
    {
        if (!value.TryCast(out T? castedValue, out string? castingError))
        {
            ErrorMessage = castingError;
            return false;
        }

        var validationResult = IsValid(castedValue!);
        if (validationResult.IsValid)
            return true;

        if (errorMessage is not null)
            ErrorMessage = errorMessage.TryFormat(out string formatted, parameters) ? formatted : errorMessage;
        else
            ErrorMessage = validationResult.Error;
        return false;
    }
}