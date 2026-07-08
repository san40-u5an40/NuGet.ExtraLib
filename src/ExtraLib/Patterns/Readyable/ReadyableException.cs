namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Исключение, ассоциированное с объектом, проверяемым на готовность
/// </summary>
/// <param name="_message">Сообщение об ошибке</param>
/// <param name="_state">Состояние объекта</param>
/// <param name="_name">Имя объекта</param>
public class ReadyableException(string? _message = null, ReadyableState? _state = null, string? _name = null) : InvalidOperationException(_message)
{
  /// <summary>
  /// Имя объекта
  /// </summary>
  public string? Name => _name;

  /// <summary>
  /// Состояние объекта
  /// </summary>
  public ReadyableState? State => _state;
}
