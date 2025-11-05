namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Статический класс, предназначенный для создания счётчика на основе замыкания
/// </summary>
public static class Counter
{
    /// <summary>
    /// Метод создания счётчика на основе замыкания
    /// </summary>
    /// <param name="startValue">Стартовое значение счётчика</param>
    /// <returns>Функция, вызов которой возвращает значение, которое затем будет инкрементированно</returns>
    public static Func<int> GetCounter(int startValue) =>
        () => startValue++;
}