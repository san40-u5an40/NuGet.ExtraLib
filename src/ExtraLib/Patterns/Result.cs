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
  private readonly bool _isValid;
  private readonly TSuccess? _success;
  private readonly TFailure? _failure;

  private Result(bool isValid, TSuccess? success, TFailure? failure) =>
      (_isValid, _success, _failure) = (isValid, success, failure);

  /// <summary>
  /// Свойство, хранящее сведенья об успешности выполненной операции, валидности результата
  /// </summary>
  public bool IsValid => _isValid;

  /// <summary>
  /// Свойство, хранящее данные при валидном результате
  /// </summary>
  public TSuccess Value => _isValid ? _success! : throw new InvalidOperationException("If the result is invalid, accessing the result value is not allowed");

  /// <summary>
  /// Свойство, хранящее данные при невалидном результате
  /// </summary>
  public TFailure Error => !_isValid ? _failure! : throw new InvalidOperationException("If the result is valid, you cannot access the error information");

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
    readyable.ToReady(success);
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
    readyable.ToNeverBeReady();
    return new(false, default, failure);
  }

  /// <summary>
  /// При невалидном результате выполняет указанное действие и завершает работу приложения
  /// </summary>
  /// <param name="action">Действие, совершаемое над Result.Error</param>
  public void ExecuteAndExitIfNotValid(Action<TFailure> action)
  {
    if (!IsValid)
    {
      action(Error);
      Environment.Exit(1);
    }
  }
}
