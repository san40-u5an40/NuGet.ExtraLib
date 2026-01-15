namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Обобщённый класс для реализации Result-паттерна
/// </summary>
/// <typeparam name="TSuccess">Тип данных, хранящихся в Value при валидном результате</typeparam>
/// <typeparam name="TFailure">Тип данных, хранящийся в Error при невалидном результате</typeparam>
/// <exception cref="InvalidOperationException">Обращение к сведеньям об ошибке при валидном результате и наоборот не допустимо</exception>
public class Result<TSuccess, TFailure>
    where TSuccess : class
    where TFailure : class
{
    private readonly bool isValid;
    private readonly TSuccess? success;
    private readonly TFailure? failure;

    private Result(bool isValid, TSuccess? success, TFailure? failure) =>
        (this.isValid, this.success, this.failure) = (isValid, success, failure);

    /// <summary>
    /// Свойство, хранящее сведенья об успешности выполненной операции, валидности результата
    /// </summary>
    public bool IsValid => isValid;
    
    /// <summary>
    /// Свойство, хранящее данные при валидном результате
    /// </summary>
    public TSuccess Value => isValid ? success! : throw new InvalidOperationException("При невалидном результате обращение к значению результата не допустимо");

    /// <summary>
    /// Свойство, хранящее данные при невалидном результате
    /// </summary>
    public TFailure Error => !isValid ? failure! : throw new InvalidOperationException("При валидном результате обращение к сведеньям об ошибке не допустимо");

    /// <summary>
    /// Статический метод создания валидного результата
    /// </summary>
    /// <param name="success">Данные валидного результата</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateSuccess(TSuccess success) =>
        new(true, success, null);

    /// <summary>
    /// Статический метод создания невалидного результата
    /// </summary>
    /// <param name="failure">Данные невалидного результата</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateFailure(TFailure failure) =>
        new(false, null, failure);
}