namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Исключение, ассоциированное с проверкой объекта на валидность при помощи Verifier'а
/// </summary>
/// <typeparam name="T">Тип валидируемого объекта</typeparam>
public class VerifierException<T> : Exception
    where T : notnull
{
    /// <summary>
    /// Конструктор исключения
    /// </summary>
    /// <param name="errors">Сведенья о невалидном объекте</param>
    /// <param name="validationObject">Невалидный объект</param>
    public VerifierException(IReadOnlyList<ValidationResult> errors, T validationObject) =>
        (Errors, ValidationObject) = (errors, validationObject);

    /// <summary>
    /// Сведенья о невалидном объекте
    /// </summary>
    public IReadOnlyList<ValidationResult> Errors { get; private init; }

    /// <summary>
    /// Невалидный объект
    /// </summary>
    public T ValidationObject { get; private init; }
}