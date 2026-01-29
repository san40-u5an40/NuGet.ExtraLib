using san40_u5an40.ExtraLib.Broad.Patterns;

// Информация об операции, хранящейся в цепочке
internal class OperationInfo<TInput, TOutput, TError> : IInvokable<TError>
    where TInput : notnull
    where TOutput : notnull
    where TError : notnull
{
    private readonly Lock _lockObg = new();
    private IReadyable<TOutput>? _outParameter = null;

    // Конструктор для вызова без out-параметров
    internal OperationInfo(Func<TInput, Result<TOutput, TError>> func) =>
        (Function, InputType, OutputType) = (func, typeof(TInput), typeof(TOutput));
    
    // Конструктор для вызова с out-параметром
    internal OperationInfo(Func<TInput, Result<TOutput, TError>> func, out Readyable<TOutput> outParameter)
    {
        (Function, InputType, OutputType) = (func, typeof(TInput), typeof(TOutput));
        _outParameter = outParameter = new Readyable<TOutput>(Function.Method.Name);
    }

    // Сама функция, принимающая какое-то значение, и возвращающая значение, которое можно оценить на валидность
    internal Func<TInput, Result<TOutput, TError>> Function { get; private init; }

    // Тип входных данных
    public Type InputType { get; private init; }

    // Тип выходных данных
    public Type OutputType { get; private init; }

    // Имя методы
    public string Name => Function.Method.Name;

    // Вызов хранимой функции в зависимости от наличия out-параметра
    public Result<object, TError> Invoke(object input)
    {
        return _outParameter == null ?
            InvokeWithoutOutParameter(input) : 
            InvokeWithOutParameter(input);
    }

    public Result<object, TError> InvokeWithOutParameter(object input)
    {
        TInput typedInput = (TInput)input;
        var functionOutput = Function(typedInput);

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

    public Result<object, TError> InvokeWithoutOutParameter(object input)
    {
        TInput typedInput = (TInput)input;
        var functionOutput = Function(typedInput);

        return functionOutput.IsValid ?
            Result<object, TError>.CreateSuccess(functionOutput.Value) :
            Result<object, TError>.CreateFailure(functionOutput.Error);
    }
}