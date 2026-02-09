namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Цепочка вызова асинхронных операций, передающая указанные данные
/// </summary>
/// <typeparam name="TInputData">Данные, которые подаются в начало цепочки</typeparam>
/// <typeparam name="TOutputData">Данные, которые цепочка возвращает в самом конце</typeparam>
/// <typeparam name="TError">Тип ошибки, которую возвращает цепочка при невалидном результате</typeparam>
/// <param name="startData">Начальные данные цепочки</param>
/// <param name="cancellationToken">Токен завершения</param>
public class AsyncChain<TInputData, TOutputData, TError>(TInputData startData, CancellationToken? cancellationToken = null)
    where TInputData : notnull
    where TOutputData : notnull
    where TError : notnull
{
    private LinkedList<IAsyncInvokable<TError>> asyncOperations = new();

    /// <summary>
    /// Метод для добавления асинхронных функции или метода в цепочку
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="funcWithoutCancellationToken">Вызываемая функция</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(funcWithoutCancellationToken));

    /// <summary>
    /// Метод для добавления асинхронных функции или метода в цепочку
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="funcWithCancellationToken">Вызываемая функция</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, CancellationToken, Task<Result<TOutput, TError>>> funcWithCancellationToken)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(funcWithCancellationToken));

    /// <summary>
    /// Метод для добавления асинхронных функции или метода в цепочку, принимающий out-параметр
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="funcWithoutCancellationToken">Вызываемая функция</param>
    /// <param name="outParameter">Возвращаемое функцией значение в формате Readyable</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Task<Result<TOutput, TError>>> funcWithoutCancellationToken, out Readyable<TOutput> outParameter)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(funcWithoutCancellationToken, out outParameter));

    /// <summary>
    /// Метод для добавления асинхронных функции или метода в цепочку, принимающий out-параметр
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="funcWithCancellationToken">Вызываемая функция</param>
    /// <param name="outParameter">Возвращаемое функцией значение в формате Readyable</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, CancellationToken, Task<Result<TOutput, TError>>> funcWithCancellationToken, out Readyable<TOutput> outParameter)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(funcWithCancellationToken, out outParameter));

    // Общая часть для двух перегрузок метода AddMethod
    // Проверяет операцию на валидность
    // И если всё хорошо, то добавляет её в конец списка
    private AsyncChain<TInputData, TOutputData, TError> ThrowIfInvalidAndAddLast<TInput, TOutput>(AsyncOperationInfo<TInput, TOutput, TError> asyncOperation)
        where TInput : notnull
        where TOutput : notnull
    {
        Type lastOutputType = asyncOperations.Count > 0 ? asyncOperations.Last!.Value.OutputType : startData.GetType();
        Type operationInputType = asyncOperation.InputType;

        if (lastOutputType != operationInputType)
            throw new FormatException($"The input parameters for the operation \"{asyncOperation.Name}\" do not meet the requirements");

        asyncOperations.AddLast(asyncOperation);
        return this;
    }

    /// <summary>
    /// Асинхронное выполнение цепочки операций
    /// </summary>
    /// <returns>
    /// Валидируемый результат:\
    /// - Если валидный, то свойство Value хранит TOutputData.\
    /// - Если невалидный, то в свойстве Error.Value хранится TError, а в Error.Type тип невалидного результата, представленный InvalidAsyncChainResultType.
    /// </returns>
    public async Task<Result<TOutputData, InvalidAsyncChainResult<TError>>> ExecuteAsync()
    {
        ThrowIfInvalidList();

        object data = startData;

        foreach (IAsyncInvokable<TError> asyncOperation in asyncOperations)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
                return Result<TOutputData, InvalidAsyncChainResult<TError>>.CreateFailure(new InvalidAsyncChainResult<TError>(InvalidAsyncChainResultType.CancellationTokenRequested));

            var result = await asyncOperation.InvokeAsync(data, cancellationToken);

            if (!result.IsValid)
                return Result<TOutputData, InvalidAsyncChainResult<TError>>.CreateFailure(new InvalidAsyncChainResult<TError>(InvalidAsyncChainResultType.NotValidMethodResult, result.Error));

            data = result.Value;
        }

        return Result<TOutputData, InvalidAsyncChainResult<TError>>.CreateSuccess((TOutputData)data);
    }

    // Проверка списка операций на валидность
    private void ThrowIfInvalidList()
    {
        if (asyncOperations.Count == 0)
            throw new InvalidOperationException("To perform a chain of operations, you must first add operations");

        if (asyncOperations.Last!.Value.OutputType != typeof(TOutputData))
            throw new FormatException($"The last type is expected to have a \"{typeof(TOutputData).Name}\"");

        if (asyncOperations.Any(p => p.IsContainsCancellationTokenParameter) && cancellationToken is null)
            throw new FormatException("When using methods with a completion token, you need to specify the token in the chain constructor");
    }
}