namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Перечисление, отражающее тип операции, производимой над счётчиком
/// </summary>
public enum CounterOperationType : ushort
{
    SetValidator,
    Increment,
    Decrement,
}