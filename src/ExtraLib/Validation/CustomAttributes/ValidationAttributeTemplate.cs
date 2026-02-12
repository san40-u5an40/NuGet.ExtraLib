namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Шаблон для создания атрибутов валидации
/// </summary>
/// <typeparam name="TAttributeTarget">Тип проверяемого свойства или класса</typeparam>
public abstract class ValidationAttributeTemplate<TAttributeTarget>(string? errorMessage = null, params object[] parameters) : ValidationAttribute
    where TAttributeTarget : class
{
    private protected abstract (bool IsValid, string? Error) IsValidTargetValue(TAttributeTarget validable);
    private protected abstract (bool IsValid, string? Error) IsValidAttributeParameters();

    /// <summary>
    /// Проверка валидности указанного значения при помощи атрибута
    /// </summary>
    /// <param name="value">Проверяемое значение</param>
    /// <returns>Логическая переменная, показывающая валидность проверяемого значения</returns>
    public sealed override bool IsValid(object? value)
    {
        if (parameters.Any(p => p.IsNull()))
            throw new AttributeParametersException(parameters, "Attribute values cannot have null values");

        var parametersValidationResult = IsValidAttributeParameters();
        if (!parametersValidationResult.IsValid)
            throw new AttributeParametersException(parameters, parametersValidationResult.Error ?? "Incorrect attribute parameters");

        if (!value.TryCast(out TAttributeTarget? castedValue, out string? castingError))
        {
            ErrorMessage = castingError;
            return false;
        }

        var validationResult = IsValidTargetValue(castedValue);
        if (validationResult.IsValid)
            return true;

        if (errorMessage.IsNotNull())
            ErrorMessage = errorMessage.TryFormat(out string formatted, parameters) ? formatted : errorMessage;
        else
            ErrorMessage = validationResult.Error ?? "Validation error";
        return false;
    }
}