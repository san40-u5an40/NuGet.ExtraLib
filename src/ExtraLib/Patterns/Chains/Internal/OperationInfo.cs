// Информация об операции, хранящейся в цепочке
internal class OperationInfo<TInput, TOutput, TError> : IInvokable<TError>
    where TInput : notnull
    where TOutput : notnull
    where TError : notnull
{
  private IReadyable<TOutput>? _outParameter = null;
  private Action<TError>? _errorHandler;

  // Конструктор для вызова без out-параметров
  internal OperationInfo(Func<TInput, Result<TOutput, TError>> func, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5) =>
      (Function, InputType, OutputType, IsLoop, _errorHandler, Attempts) = (func, typeof(TInput), typeof(TOutput), isLoop, errorHandler, attempts);

  // Конструктор для вызова с out-параметром
  internal OperationInfo(Func<TInput, Result<TOutput, TError>> func, out Readyable<TOutput> outParameter, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5)
  {
    (Function, InputType, OutputType, IsLoop, _errorHandler, Attempts) = (func, typeof(TInput), typeof(TOutput), isLoop, errorHandler, attempts);
    _outParameter = outParameter = new Readyable<TOutput>(Function.Method.Name);
  }

  // Сама функция, принимающая какое-то значение, и возвращающая значение, которое можно оценить на валидность
  internal Func<TInput, Result<TOutput, TError>> Function { get; private init; }

  // Тип входных данных
  public Type InputType { get; private init; }

  // Тип выходных данных
  public Type OutputType { get; private init; }

  // Количество попыток при зацикливании
  public uint Attempts { get; private init; }

  // Зациклить ли операцию, пока не будет валидный результат, или пока не исчерпаются попытки
  public bool IsLoop { get; private init; }

  // Имя методы
  public string Name => Function.Method.Name;

  // Вызов хранимой функции в зависимости от наличия out-параметра
  public Result<object, TError> Invoke(object input)
  {
    bool isOutParameterContain = _outParameter != null;
    return Invoke(input, isOutParameterContain);
  }

  private Result<object, TError> Invoke(object input, bool isOutParameterContain)
  {
    TInput typedInput = (TInput)input;
    Result<TOutput, TError> functionOutput;

    if (IsLoop)
    {
      // Через do-while потому что компилятор не знает, что Attempts >= 1 (если бы был for)
      int i = 0;
      do
      {
        functionOutput = Function(typedInput);
        if (functionOutput.IsValid)
          break;
        _errorHandler!(functionOutput.Error);
      }
      while (++i < Attempts);
    }
    else
      functionOutput = Function(typedInput);

    if (functionOutput.IsValid)
    {
      if (isOutParameterContain)
        _outParameter!.ToReady(functionOutput.Value);
      return Result<object, TError>.CreateSuccess(functionOutput.Value);
    }
    else
    {
      if (isOutParameterContain)
        _outParameter!.ToNeverBeReady();
      return Result<object, TError>.CreateFailure(functionOutput.Error);
    }
  }
}
