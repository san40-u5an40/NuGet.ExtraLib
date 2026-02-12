namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Класс для проверки валидируемых объектов
/// </summary>
public static class Verifier
{
    /// <summary>
    /// Проверка валидируемых объектов
    /// </summary>
    /// <typeparam name="T">Тип проверяемого объекта</typeparam>
    /// <param name="validationObject">Объект для проверки валидности</param>
    /// <returns>Результат проверки в формате объекта Result</returns>
    public static Result<T, IReadOnlyList<ValidationResult>> Check<T>(T validationObject)
        where T : notnull
    {
        var validationResult = GetValidInformation(validationObject);

        if (validationResult.IsValid)
            return Result<T, IReadOnlyList<ValidationResult>>.CreateSuccess(validationObject);
        else
            return Result<T, IReadOnlyList<ValidationResult>>.CreateFailure(validationResult.Errors);
    }

    /// <summary>
    /// Бросает исключение, если объект не валиден
    /// </summary>
    /// <typeparam name="T">Тип проверяемого объекта</typeparam>
    /// <param name="validationObject">Объект для проверки валидности</param>
    /// <exception cref="VerifierException{T}">Исключение, связанное с невалиднмы объектом</exception>
    public static void ThrowIfNotValid<T>(T validationObject)
        where T : notnull
    {
        var validationResult = GetValidInformation(validationObject);

        if (validationResult.IsValid)
            return;
        else
            throw new VerifierException<T>(validationResult.Errors, validationObject);
    }

    private static (bool IsValid, IReadOnlyList<ValidationResult> Errors) GetValidInformation<T>(T validationObject)
        where T : notnull
    {
        var context = new ValidationContext(validationObject);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(validationObject, context, results, true))
            return (false, results.AsReadOnly());
        else
            return (true, []);
    }
}