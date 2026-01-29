namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Интерфейс для объектов, проверяемых на готовность
/// </summary>
/// <typeparam name="TValue">Тип хранимого значения</typeparam>
public interface IReadyable<TValue>
    where TValue : notnull
{
    public string Name { get; }
    public TValue Value { get; set; }
    public ReadyableState State { get; }
    public bool IsInitialized { get; }
    public bool IsWaiting { get; }
    public bool IsReady { get; }
    public bool IsNeverBeReady { get; }
    public void ThrowIfNotWaiting();
    public void ThrowIfNotWaiting(string message);
    public void ThrowIfNotReady();
    public void ThrowIfNotReady(string message);
    public void ThrowIfNotInitialized();
    public void ThrowIfNotInitialized(string message);
    public void ToReady();
    public void ToNeverBeReady();
}