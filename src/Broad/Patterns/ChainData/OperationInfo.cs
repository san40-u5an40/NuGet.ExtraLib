using san40_u5an40.ExtraLib.Broad.Patterns;

// Информация об операции, хранящейся в цепочке
internal class OperationInfo<TInput, TOutput, TError> : IInvokable<TError>
    where TInput : class
    where TOutput : class
    where TError : class
{
    internal OperationInfo(Func<TInput, IValidable<TOutput, TError>> func) =>
        (Function, InputType, OutputType) = (func, typeof(TInput), typeof(TOutput));

    // Сама функция, принимающая какое-то значение, и возвращающая значение, которое можно оценить на валидность
    internal Func<TInput, IValidable<TOutput, TError>> Function { get; private init; }

    // Тип входных данных
    public Type InputType { get; private init; }

    // Тип выходных данных
    public Type OutputType { get; private init; }

    // Имя методы
    public string Name => Function.Method.Name;

    // Вызов хранимого метода, возвращающий валидируемое значение
    public IValidable<object, TError> Invoke(object input)
    {
        TInput typedInput = (TInput)input;
        var functionOutput = Function(typedInput);

        return functionOutput.IsValid ?
            Result<object, TError>.CreateSuccess(functionOutput.Value) :
            Result<object, TError>.CreateFailure(functionOutput.Error);
    }
}