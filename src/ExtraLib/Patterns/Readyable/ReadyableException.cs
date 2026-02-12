namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Исключение, ассоциированное с объектом, проверяемым на готовность
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
/// <param name="state">Состояние объекта</param>
/// <param name="name">Имя объекта</param>
public class ReadyableException(string? message = null, ReadyableState? state = null, string? name = null) : InvalidOperationException(message)
{
    /// <summary>
    /// Имя объекта
    /// </summary>
    public string? Name => name;

    /// <summary>
    /// Состояние объекта
    /// </summary>
    public ReadyableState? State => state;
}