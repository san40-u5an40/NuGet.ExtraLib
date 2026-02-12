namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Делегат для подписки на обновления счётчика
/// </summary>
/// <param name="type">Тип операции, производимый над счётчиком</param>
/// <param name="step">Количество, на которое изменился счётчик</param>
/// <param name="counter">Источник изменений</param>
public delegate void CounterEventHandler(CounterOperationType type, long step, Counter counter);