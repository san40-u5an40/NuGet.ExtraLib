namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Обобщённый класс для реализации Result-паттерна
/// </summary>
/// <typeparam name="TSuccess">Тип данных, хранящихся в Value при валидном результате</typeparam>
/// <typeparam name="TFailure">Тип данных, хранящийся в Error при невалидном результате</typeparam>
/// <exception cref="InvalidOperationException">Обращение к сведеньям об ошибке при валидном результате и наоборот не допустимо</exception>
public class Result<TSuccess, TFailure>
    where TSuccess : notnull
    where TFailure : notnull
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
    public TSuccess Value => isValid ? success! : throw new InvalidOperationException("If the result is invalid, accessing the result value is not allowed");

    /// <summary>
    /// Свойство, хранящее данные при невалидном результате
    /// </summary>
    public TFailure Error => !isValid ? failure! : throw new InvalidOperationException("If the result is valid, you cannot access the error information");

    /// <summary>
    /// Статический метод создания валидного результата
    /// </summary>
    /// <param name="success">Данные валидного результата</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateSuccess(TSuccess success) =>
        new(true, success, default);

    /// <summary>
    /// Статический метод создания невалидного результата
    /// </summary>
    /// <param name="failure">Данные невалидного результата</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateFailure(TFailure failure) =>
        new(false, default, failure);

    /// <summary>
    /// Статический метод создания валидного результата
    /// </summary>
    /// <param name="success">Данные валидного результата</param>
    /// <param name="readyable">Объект, проверяемый на готовность, ассоциированные с этим результатом</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateSuccess(TSuccess success, IReadyable<TSuccess> readyable)
    {
        using (new Lock().EnterScope())
        {
            readyable.ThrowIfNotWaiting();
            readyable.Value = success;
            readyable.ToReady();
        }

        return new(true, success, default);
    }

    /// <summary>
    /// Статический метод создания невалидного результата
    /// </summary>
    /// <param name="failure">Данные невалидного результата</param>
    /// <param name="readyable">Объект, проверяемый на готовность, ассоциированные с этим результатом</param>
    /// <returns>Объект, хранящий данные результата</returns>
    public static Result<TSuccess, TFailure> CreateFailure(TFailure failure, IReadyable<TSuccess> readyable)
    {
        using (new Lock().EnterScope())
        {
            readyable.ThrowIfNotWaiting();
            readyable.ToNeverBeReady();
        }
        
        return new(false, default, failure);
    }
}