namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Цепочка вызова операций, передающая указанные данные
/// </summary>
/// <typeparam name="TInputData">Данные, которые подаются в начало цепочки</typeparam>
/// <typeparam name="TOutputData">Данные, которые цепочка возвращает в самом конце</typeparam>
/// <typeparam name="TError">Тип ошибки, которую возвращает цепочка при невалидном результате</typeparam>
/// <param name="startData">Начальные данные цепочки</param>
public class Chain<TInputData, TOutputData, TError>(TInputData startData)
    where TInputData : class
    where TOutputData : class
    where TError : class
{
    private LinkedList<IInvokable<TError>> operations = new();

    /// <summary>
    /// Метод для добавления функции/метода в цепочку
    /// </summary>
    /// <typeparam name="TInput">Тип данных, которые принимает функция</typeparam>
    /// <typeparam name="TOutput">Тип возвращаемых данных функцией</typeparam>
    /// <param name="func">Вызываемая функция</param>
    /// <returns>Экземпляр цепочки (Fluent API)</returns>
    public Chain<TInputData, TOutputData, TError> AddMethod<TInput, TOutput>(Func<TInput, IValidable<TOutput, TError>> func)
        where TInput : class
        where TOutput : class
    {
        var operation = new OperationInfo<TInput, TOutput, TError>(func);

        ThrowIfInvalidOperation(operation);
        operations.AddLast(operation);

        return this;
    }

    // Проверка добавляемой функции на валидность
    private void ThrowIfInvalidOperation<TInput, TOutput>(OperationInfo<TInput, TOutput, TError> operation)
        where TInput : class
        where TOutput : class
    {
        Type lastOutputType = operations.Count > 0 ? operations.Last!.Value.OutputType : startData.GetType();
        Type operationInputType = operation.InputType;

        if (lastOutputType != operationInputType)
            throw new FormatException($"Входные параметры для операции \"{operation.Name}\" не соответствуют требованиям!");
    }

    /// <summary>
    /// Выполнение цепочки операций
    /// </summary>
    /// <returns>
    /// Валидируемый результат:<br>
    /// - Если валидный, то свойство Value хранит TOutputData.<br>
    /// - Если невалидный, то в свойстве Error хранится TError.
    /// </returns>
    public IValidable<TOutputData, TError> Execute()
    {
        ThrowIfInvalidList();

        object data = startData;

        foreach (IInvokable<TError> operation in operations)
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
        if (operations.Count == 0)
            throw new InvalidOperationException("Для исполнения цепочки операций необходимо сначала добавить операции!");

        if (operations.Last!.Value.OutputType != typeof(TOutputData))
            throw new FormatException($"От последнего типа ожидается \"{typeof(TOutputData).Name}\"!");
    }
}