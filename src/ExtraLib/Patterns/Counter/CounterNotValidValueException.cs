namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Исключение о невалидном значении счётчика
/// </summary>
public class CounterNotValidValueException : Exception
{
    public CounterNotValidValueException(Counter counter, CounterOperationType operationType, long step, string? message = null)
        : base(message) =>
        (Counter, OperationType, Step) = (counter, operationType, step);

    /// <summary>
    /// Счётчик, с которым связано исключение
    /// </summary>
    public Counter Counter { get; private init; }

    /// <summary>
    /// Операция, производимая над счётчиком
    /// </summary>
    public CounterOperationType OperationType { get; private init; }

    /// <summary>
    /// Шаг, на который изменялся счётчик
    /// </summary>
    public long Step { get; private init; }
}