using san40_u5an40.ExtraLib.Broad.Patterns;

// Информация об операции, хранящейся в цепочке
internal class AsyncOperationInfo<TInput, TOutput, TError> : IAsyncInvokable<TError>
    where TInput : notnull
    where TOutput : notnull
    where TError : notnull
{
    private readonly Lock _lockObg = new();

    private readonly string name;

    private Func<TInput, CancellationToken, Task<Result<TOutput, TError>>>? funcWithCancellationToken = null;
    private Func<TInput, Task<Result<TOutput, TError>>>? funcWithoutCancellationToken = null;

    private IReadyable<TOutput>? _outParameter = null;

    // Четыре перегрузки конструктора, с наличием/отсутствием out-параметров и токенов завершения
    internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken) =>
        (this.name, this.InputType, this.OutputType, this.funcWithoutCancellationToken) = (funcWithoutCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithoutCancellationToken);
    internal AsyncOperationInfo(Func<TInput, CancellationToken, Task<Result<TOutput, TError>>> funcWithCancellationToken) =>
        (this.name, this.InputType, this.OutputType, this.funcWithCancellationToken) = (funcWithCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithCancellationToken);
    internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken, out Readyable<TOutput> outParameter)
    {
        (this.name, this.InputType, this.OutputType, this.funcWithoutCancellationToken) = (funcWithoutCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithoutCancellationToken);
        _outParameter = outParameter = new Readyable<TOutput>(this.name);
    }
    internal AsyncOperationInfo(Func<TInput, CancellationToken, Task<Result<TOutput, TError>>> funcWithCancellationToken, out Readyable<TOutput> outParameter)
    {
        (this.name, this.InputType, this.OutputType, this.funcWithCancellationToken) = (funcWithCancellationToken.Method.Name, typeof(TInput), typeof(TOutput), funcWithCancellationToken);
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

    // Вызов хранимой функции в зависимости от наличия out-параметра
    public async Task<Result<object, TError>> InvokeAsync(object input, CancellationToken? cancellationToken = null)
    {
        return _outParameter == null ?
            await InvokeWithoutOutParameterAsync(input, cancellationToken) : 
            await InvokeWithOutParameterAsync(input, cancellationToken);
    }

    public async Task<Result<object, TError>> InvokeWithOutParameterAsync(object input, CancellationToken? cancellationToken = null)
    {
        TInput typedInput = (TInput)input;

        // Если в цепочке содержатся функции с токеном в параметре, но не содержится сам токен, будет исключение
        // А если вместе с такими функциями содержится и сам токен, то он будет передан в InvokeAsync
        var functionOutput = IsContainsCancellationTokenParameter ? await funcWithCancellationToken!(typedInput, cancellationToken!.Value) : await funcWithoutCancellationToken!(typedInput);

        lock (_lockObg)
        {
            _outParameter!.ThrowIfNotWaiting();

            if (functionOutput.IsValid)
            {
                _outParameter.Value = functionOutput.Value;
                _outParameter.ToReady();
                return Result<object, TError>.CreateSuccess(functionOutput.Value);
            }
            else
            {
                _outParameter.ToNeverBeReady();
                return Result<object, TError>.CreateFailure(functionOutput.Error);
            }
        }
    }

    public async Task<Result<object, TError>> InvokeWithoutOutParameterAsync(object input, CancellationToken? cancellationToken = null)
    {
        TInput typedInput = (TInput)input;

        // Если в цепочке содержатся функции с токеном в параметре, но не содержится сам токен, будет исключение
        // А если вместе с такими функциями содержится и сам токен, то он будет передан в InvokeAsync
        var functionOutput = IsContainsCancellationTokenParameter ? await funcWithCancellationToken!(typedInput, cancellationToken!.Value) : await funcWithoutCancellationToken!(typedInput);

        return functionOutput.IsValid ?
            Result<object, TError>.CreateSuccess(functionOutput.Value) :
            Result<object, TError>.CreateFailure(functionOutput.Error);
    }
}