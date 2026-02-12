namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Тип невалидного результата работы асинхронной цепи
/// </summary>
public enum InvalidAsyncChainResultType
{
    CancellationTokenRequested,
    NotValidMethodResult,
}