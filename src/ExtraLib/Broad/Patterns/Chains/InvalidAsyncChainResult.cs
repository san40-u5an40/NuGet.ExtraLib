namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Запись для свойства Result.Error, при выполнении асинхронной цепи
/// </summary>
/// <typeparam name="TError">Тип, которым представлен невалидный результат в свойстве Value</typeparam>
/// <param name="Type">Разновидность навалидного результата асинхронной цепи</param>
/// <param name="Value">Значение невалидного результата</param>
public record InvalidAsyncChainResult<TError>(InvalidAsyncChainResultType Type, TError? Value = default);