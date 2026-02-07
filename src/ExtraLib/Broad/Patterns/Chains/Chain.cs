namespace san40_u5an40.ExtraLib.Broad.Patterns;

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
    private LinkedList<IInvokable<TError>> operations = new();

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
        => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func));

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
        => ThrowIfInvalidAndAddLast(new OperationInfo<TInput, TOutput, TError>(func, out readyable));

    // Общая часть для двух перегрузок метода AddMethod
    // Проверяет операцию на валидность
    // И если всё хорошо, то добавляет её в конец списка
    private Chain<TInputData, TOutputData, TError> ThrowIfInvalidAndAddLast<TInput, TOutput>(OperationInfo<TInput, TOutput, TError> operation)
        where TInput : notnull
        where TOutput : notnull
    {
        Type lastOutputType = operations.Count > 0 ? operations.Last!.Value.OutputType : startData.GetType();
        Type operationInputType = operation.InputType;

        if (lastOutputType != operationInputType)
            throw new FormatException($"Входные параметры для операции \"{operation.Name}\" не соответствуют требованиям!");

        operations.AddLast(operation);
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