namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Информация о невалидном результате выполнения асинхронной цепи
/// </summary>
/// <typeparam name="TError">Тип, которым представлен невалидный результат в свойстве Value</typeparam>
public class InvalidAsyncChainResult<TError>
{
    /// <summary>
    /// Конструктор для создания информации о невалидном результате выполнения асинхронной цепи
    /// </summary>
    /// <param name="type">Разновидность навалидного результата асинхронной цепи</param>
    /// <param name="value">Значение невалидного результата</param>
    public InvalidAsyncChainResult(InvalidAsyncChainResultType type, TError? value = default) =>
        (Type, Value) = (type, value);

    /// <summary>
    /// Разновидность навалидного результата асинхронной цепи
    /// </summary>
    public InvalidAsyncChainResultType Type { get; private init; }

    /// <summary>
    /// Значение невалидного результата
    /// </summary>
    public TError? Value { get; private init; }
}