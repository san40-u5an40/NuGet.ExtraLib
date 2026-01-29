using san40_u5an40.ExtraLib.Broad.Patterns;

// Информация об операции, хранящейся в цепочке
internal class AsyncOperationInfo<TInput, TOutput, TError> : IAsyncInvokable<TError>
    where TInput : notnull
    where TOutput : notnull
    where TError : notnull
{
    private readonly Lock _lockObg = new();
    private IReadyable<TOutput>? _outParameter = null;

    // Конструктор для вызова без out-параметров
    internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> asyncFunc) =>
        (AsyncFunction, InputType, OutputType) = (asyncFunc, typeof(TInput), typeof(TOutput));
    
    // Конструктор для вызова с out-параметром
    internal AsyncOperationInfo(Func<TInput, Task<Result<TOutput, TError>>> asyncFunc, out Readyable<TOutput> outParameter)
    {
        (AsyncFunction, InputType, OutputType) = (asyncFunc, typeof(TInput), typeof(TOutput));
        _outParameter = outParameter = new Readyable<TOutput>(AsyncFunction.Method.Name);
    }

    // Сама функция, принимающая какое-то значение, и возвращающая значение, которое можно оценить на валидность
    internal Func<TInput, Task<Result<TOutput, TError>>> AsyncFunction { get; private init; }

    // Тип входных данных
    public Type InputType { get; private init; }

    // Тип выходных данных
    public Type OutputType { get; private init; }

    // Имя методы
    public string Name => AsyncFunction.Method.Name;

    // Вызов хранимой функции в зависимости от наличия out-параметра
    public async Task<Result<object, TError>> InvokeAsync(object input)
    {
        return _outParameter == null ?
            await InvokeWithoutOutParameterAsync(input) : 
            await InvokeWithOutParameterAsync(input);
    }

    public async Task<Result<object, TError>> InvokeWithOutParameterAsync(object input)
    {
        TInput typedInput = (TInput)input;
        var functionOutput = await AsyncFunction(typedInput);

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

    public async Task<Result<object, TError>> InvokeWithoutOutParameterAsync(object input)
    {
        TInput typedInput = (TInput)input;
        var functionOutput = await AsyncFunction(typedInput);

        return functionOutput.IsValid ?
            Result<object, TError>.CreateSuccess(functionOutput.Value) :
            Result<object, TError>.CreateFailure(functionOutput.Error);
    }
}