namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Интерфейс объекта, который можно проверить на валидность
/// </summary>
public interface IValidable<out TSuccess, out TFailure>
{
    bool IsValid { get; }
    TSuccess Value { get; }
    TFailure Error { get; }
}