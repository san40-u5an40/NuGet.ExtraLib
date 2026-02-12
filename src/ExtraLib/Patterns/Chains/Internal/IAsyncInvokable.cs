// Интерфейс для вызываемой операции, хранящейся в цепочке
internal interface IAsyncInvokable<TError> : IChainOperation
    where TError : notnull
{
    internal Task<Result<object, TError>> InvokeAsync(object input, CancellationToken? cancellationToken = null);
    internal bool IsContainsCancellationTokenParameter { get; }
}