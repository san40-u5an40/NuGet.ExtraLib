// Интерфейс для для операции, хранящейся в цепочке
internal interface IChainOperation
{
    internal Type InputType { get; }
    internal Type OutputType { get; }
    internal string Name { get; }
}