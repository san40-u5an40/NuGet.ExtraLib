// Интерфейс для для вызываемой операции, хранящейся в цепочке
internal interface IInvokable<TError> : IChainOperation
    where TError : notnull
{
    internal Result<object, TError> Invoke(object input);
}