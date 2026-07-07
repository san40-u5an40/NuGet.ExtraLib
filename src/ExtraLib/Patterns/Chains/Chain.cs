namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Цепочка вызова операций, передающая указанные данные
/// </summary>
/// <typeparam name="TInputData">Данные, которые подаются в начало цепочки</typeparam>
/// <typeparam name="TOutputData">Данные, которые цепочка возвращает в самом конце</typeparam>
/// <typeparam name="TError">Тип ошибки, которую возвращает цепочка при невалидном результате</typeparam>
/// <param name="startData">Начальные данные цепочки</param>
public class Chain<TInputData, TOutputData, TError>(TInputData startData)
    where TInputData : notnull
    where TOutputData : notnull
    where TError : notnull
{
  private readonly LinkedList<IInvokable<TError>> _operations = new();

  /// <summary>
  /// Метод для добавления функции/метода в цепочку
  /// </summary>
  /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
  /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
  /// <param name="func">Вызываемая функция</param>
  /// <returns>Экземпляр цепочки (Fluent API)</returns>
  public Chain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Result<TOutput, TError>> func)
      where TInput : notnull
      where TOutput : notnull
      => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func, false));

  /// <summary>
  /// Метод для добавления функции/метода в цепочку с out-параметром
  /// </summary>
  /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
  /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
  /// <param name="func">Вызываемая функция</param>
  /// <param name="readyable">Возвращаемое функцией значение в формате Readyable</param>
  /// <returns>Экземпляр цепочки (Fluent API)</returns>
  public Chain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Result<TOutput, TError>> func, out Readyable<TOutput> readyable)
      where TInput : notnull
      where TOutput : notnull
      => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func, out readyable, false));

  /// <summary>
  /// Метод для добавления зацикленной функции в цепочку
  /// </summary>
  /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
  /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
  /// <param name="func">Вызываемая функция</param>
  /// <param name="errorHandler">Обработчик невалидных результатов</param>
  /// <param name="attempts">Количество попыток для валидного завершения функции</param>
  /// <returns>Экземпляр цепочки (Fluent API)</returns>
  public Chain<TInputData, TOutputData, TError> AddLoop<TInput, TOutput>(Func<TInput, Result<TOutput, TError>> func, Action<TError> errorHandler, uint attempts = 5)
      where TInput : notnull
      where TOutput : notnull
      => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func, true, errorHandler, attempts));

  /// <summary>
  /// Метод для добавления зацикленной функции в цепочку с out-параметром
  /// </summary>
  /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
  /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
  /// <param name="func">Вызываемая функция</param>
  /// <param name="readyable">Возвращаемое функцией значение в формате Readyable</param>
  /// <param name="errorHandler">Обработчик невалидных результатов</param>
  /// <param name="attempts">Количество попыток для валидного завершения функции</param>
  /// <returns>Экземпляр цепочки (Fluent API)</returns>
  public Chain<TInputData, TOutputData, TError> AddLoop<TInput, TOutput>(Func<TInput, Result<TOutput, TError>> func, out Readyable<TOutput> readyable, Action<TError> errorHandler, uint attempts = 5)
      where TInput : notnull
      where TOutput : notnull
      => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func, out readyable, true, errorHandler, attempts));

  // Общая часть для двух перегрузок метода AddMethod
  // Проверяет операцию на валидность
  // И если всё хорошо, то добавляет её в конец списка
  private Chain<TInputData, TOutputData, TError> ThrowIfInvalidAndAddLast<TInput, TOutput>(OperationInfo<TInput, TOutput, TError> operation)
      where TInput : notnull
      where TOutput : notnull
  {
    Type lastOutputType = _operations.Count > 0 ? _operations.Last!.Value.OutputType : typeof(TInputData);
    Type operationInputType = operation.InputType;

    if (lastOutputType != operationInputType)
      throw new FormatException($"The input parameters for the operation \"{operation.Name}\" do not meet the requirements");

    if (operation.IsLoop && operation.Attempts <= 0)
      throw new FormatException($"Number of attempts less than 1 for \"{operation.Name}\" method is not allowed");

    _operations.AddLast(operation);
    return this;
  }

  /// <summary>
  /// Выполнение цепочки операций
  /// </summary>
  /// <returns>
  /// Валидируемый результат:\
  /// - Если валидный, то свойство Value хранит TOutputData.\
  /// - Если невалидный, то в свойстве Error хранится TError.
  /// </returns>
  public Result<TOutputData, TError> Execute()
  {
    ThrowIfInvalidList();

    object data = startData;

    foreach (IInvokable<TError> operation in _operations)
    {
      var result = operation.Invoke(data);

      if (!result.IsValid)
        return Result<TOutputData, TError>.CreateFailure(result.Error);

      data = result.Value;
    }

    return Result<TOutputData, TError>.CreateSuccess((TOutputData)data);
  }

  // Проверка списка операций на валидность
  private void ThrowIfInvalidList()
  {
    if (_operations.Count == 0)
      throw new InvalidOperationException("To perform a chain of operations, you must first add operations");

    if (_operations.Last!.Value.OutputType != typeof(TOutputData))
      throw new FormatException($"The last type is expected to have a \"{typeof(TOutputData).Name}\"");
  }
}
