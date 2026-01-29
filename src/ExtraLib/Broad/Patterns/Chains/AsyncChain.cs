namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Цепочка вызова асинхронных операций, передающая указанные данные
/// </summary>
/// <typeparam name="TInputData">Данные, которые подаются в начало цепочки</typeparam>
/// <typeparam name="TOutputData">Данные, которые цепочка возвращает в самом конце</typeparam>
/// <typeparam name="TError">Тип ошибки, которую возвращает цепочка при невалидном результате</typeparam>
/// <param name="startData">Начальные данные цепочки</param>
public class AsyncChain<TInputData, TOutputData, TError>(TInputData startData)
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
    /// <param name="asyncFunc">Вызываемая функция</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Task<Result<TOutput, TError>>> asyncFunc)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(asyncFunc));

    /// <summary>
    /// Метод для добавления асинхронных функции или метода в цепочку, принимающий out-параметр
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="asyncFunc">Вызываемая функция</param>
    /// <param name="readyable">Возвращаемое функцией значение в формате Readyable</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public AsyncChain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, Task<Result<TOutput, TError>>> asyncFunc, out Readyable<TOutput> readyable)
        where TInput : notnull
        where TOutput : notnull
        => ThrowIfInvalidAndAddLast(new AsyncOperationInfo<TInput, TOutput, TError>(asyncFunc, out readyable));

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
            throw new FormatException($"Входные параметры для операции \"{asyncOperation.Name}\" не соответствуют требованиям!");

        asyncOperations.AddLast(asyncOperation);
        return this;
    }

    /// <summary>
    /// Асинхронное выполнение цепочки операций
    /// </summary>
    /// <returns>
    /// Валидируемый результат:<br>
    /// - Если валидный, то свойство Value хранит TOutputData.<br>
    /// - Если невалидный, то в свойстве Error хранится TError.
    /// </returns>
    public async Task<Result<TOutputData, TError>> ExecuteAsync()
    {
        ThrowIfInvalidList();

        object data = startData;

        foreach (IAsyncInvokable<TError> asyncOperation in asyncOperations)
        {
            var result = await asyncOperation.InvokeAsync(data);

            if (!result.IsValid)
                return Result<TOutputData, TError>.CreateFailure(result.Error);

            data = result.Value;
        }

        return Result<TOutputData, TError>.CreateSuccess((TOutputData)data);
    }

    // Проверка списка операций на валидность
    private void ThrowIfInvalidList()
    {
        if (asyncOperations.Count == 0)
            throw new InvalidOperationException("Для исполнения цепочки операций необходимо сначала добавить операции!");

        if (asyncOperations.Last!.Value.OutputType != typeof(TOutputData))
            throw new FormatException($"От последнего типа ожидается \"{typeof(TOutputData).Name}\"!");
    }
}