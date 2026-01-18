using san40_u5an40.ExtraLib.Broad.Patterns;

// Интерфейс для операции, хранящейся в цепочке
internal interface IInvokable<TError>
    where TError : class
{
    internal IValidable<object, TError> Invoke(object input);
    internal Type InputType { get; }
    internal Type OutputType { get; }
    internal string Name { get; }
}