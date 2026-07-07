// Информация об операции, хранящейся в цепочке
internal class AsyncOperationInfo<TInput, TOutput, TError> : IAsyncInvokable<TError>
    where TInput : notnull
    where TOutput : notnull
    where TError : notnull
{
  private readonly string name;

  private readonly Func<TInput, CancellationToken?, Task<Result<TOutput, TError>>>? funcWithCancellationToken = null;
  private readonly Func<TInput, Task<Result<TOutput, TError>>>? funcWithoutCancellationToken = null;

  private readonly Action<TError>? _errorHandler;

  private readonly IReadyable<TOutput>? _outParameter = null;

  // Четыре перегрузки конструктора, с наличием/отсутствием out-параметров и токенов завершения
  internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5) =>
      (this.name, this.InputType, this.OutputType, this.funcWithoutCancellationToken, IsLoop, _errorHandler, Attempts) = (funcWithoutCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithoutCancellationToken, isLoop, errorHandler, attempts);
  internal AsyncOperationInfo(Func<TInput, CancellationToken?, Task<Result<TOutput, TError>>> funcWithCancellationToken, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5) =>
      (this.name, this.InputType, this.OutputType, this.funcWithCancellationToken, IsLoop, _errorHandler, Attempts) = (funcWithCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithCancellationToken, isLoop, errorHandler, attempts);
  internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken, out Readyable<TOutput> outParameter, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5)
  {
    (this.name, this.InputType, this.OutputType, this.funcWithoutCancellationToken, IsLoop, _errorHandler, Attempts) = (funcWithoutCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithoutCancellationToken, isLoop, errorHandler, attempts);
    _outParameter = outParameter = new Readyable<TOutput>(this.name);
  }
  internal AsyncOperationInfo(Func<TInput, CancellationToken?, Task<Result<TOutput, TError>>> funcWithCancellationToken, out Readyable<TOutput> outParameter, bool isLoop, Action<TError>? errorHandler = null, uint attempts = 5)
  {
    (this.name, this.InputType, this.OutputType, this.funcWithCancellationToken, IsLoop, _errorHandler, Attempts) = (funcWithCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithCancellationToken, isLoop, errorHandler, attempts);
    _outParameter = outParameter = new Readyable<TOutput>(this.name);
  }

  // Имя методы
  public string Name => name;

  // Содержится ли в параметрах метода токен для завершения
  public bool IsContainsCancellationTokenParameter => funcWithCancellationToken is not null;

  // Тип входных данных
  public Type InputType { get; private init; }

  // Тип выходных данных
  public Type OutputType { get; private init; }
  //
  // Количество попыток при зацикливании
  public uint Attempts { get; private init; }

  // Зациклить ли операцию, пока не будет валидный результат, или пока не исчерпаются попытки
  public bool IsLoop { get; private init; }

  // Вызов хранимой функции в зависимости от наличия out-параметра
  public async Task<Result<object, TError>> InvokeAsync(object input, CancellationToken? cancellationToken = null)
  {
    bool isOutParameterContain = _outParameter != null;
    return await Invoke(input, cancellationToken, isOutParameterContain);
  }

  private async Task<Result<object, TError>> Invoke(object input, CancellationToken? cancellationToken, bool isOutParameterContain)
  {
    TInput typedInput = (TInput)input;
    Result<TOutput, TError> functionOutput;

    if (IsLoop)
    {
      // Через do-while потому что компилятор не знает, что Attempts >= 1 (если бы был for)
      int i = 0;
      do
      {
        functionOutput = IsContainsCancellationTokenParameter ? await funcWithCancellationToken!(typedInput, cancellationToken) : await funcWithoutCancellationToken!(typedInput);
        if (functionOutput.IsValid)
          break;
        _errorHandler!(functionOutput.Error);
      }
      while (++i < Attempts);
    }
    else
      functionOutput = IsContainsCancellationTokenParameter ? await funcWithCancellationToken!(typedInput, cancellationToken) : await funcWithoutCancellationToken!(typedInput);

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
