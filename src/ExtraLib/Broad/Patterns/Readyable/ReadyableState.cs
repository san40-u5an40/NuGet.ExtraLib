namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Перечисление состояний объекта, проверяемого на готовность
/// </summary>
public enum ReadyableState : short
{
    NeverBeReady = -1,
    Waiting      =  0,
    Ready        =  1
}