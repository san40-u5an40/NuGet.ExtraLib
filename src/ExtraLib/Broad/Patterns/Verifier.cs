namespace san40_u5an40.ExtraLib.Broad.Patterns;

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
        var context = new ValidationContext(validationObject);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(validationObject, context, results, true))
            return Result<T, IReadOnlyList<ValidationResult>>.CreateFailure(results.AsReadOnly());
        else
            return Result<T, IReadOnlyList<ValidationResult>>.CreateSuccess(validationObject);
    }
}