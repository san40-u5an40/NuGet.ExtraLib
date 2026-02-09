namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Методы расширения для Object
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Попытка преобразовать к обобщённому типу
    /// </summary>
    /// <typeparam name="T">Тип для преобразования</typeparam>
    /// <param name="obj">Объект, для преобразования</param>
    /// <param name="castedValue">Преобразованный объект</param>
    /// <returns>Логическое значение, отражающее успешно ли был преобразован объект</returns>
    public static bool TryCast<T>(this object? obj, out T? castedValue)
        where T : class
    {
        castedValue = obj as T;
        return castedValue is not null;
    }

    /// <summary>
    /// Попытка преобразовать к обобщённому типу и формирование сообщения об ошибке в случае неудачи
    /// </summary>
    /// <typeparam name="T">Тип для преобразования</typeparam>
    /// <param name="obj">Объект, для преобразования</param>
    /// <param name="castedValue">Преобразованный объект</param>
    /// <param name="errorMessage">Сообщение об ошибке приведения типа</param>
    /// <returns>Логическое значение, отражающее успешно ли был преобразован объект</returns>
    public static bool TryCast<T>(this object? obj, out T? castedValue, out string? errorMessage)
        where T : class
    {
        bool isCasted = obj.TryCast(out castedValue);
        errorMessage = isCasted ? null : $"Specified value is not a {typeof(T).Name}";
        return isCasted;
    }
}