using san40_u5an40.ExtraLib.Broad.Patterns;

// Интерфейс для для вызываемой операции, хранящейся в цепочке
internal interface IInvokable<TError> : IChainOperation
    where TError : notnull
{
    internal Result<object, TError> Invoke(object input);
}