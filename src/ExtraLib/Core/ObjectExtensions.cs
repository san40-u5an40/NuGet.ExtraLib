namespace san40_u5an40.ExtraLib.Core;

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
    [MemberNotNullWhen(true)]
    public static bool TryCast<T>([NotNullWhen(true)] this object? obj, [NotNullWhen(true)] out T? castedValue)
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
    [MemberNotNullWhen(true)]
    public static bool TryCast<T>([NotNullWhen(true)] this object? obj, [NotNullWhen(true)] out T? castedValue, [NotNullWhen(false)] out string? errorMessage)
        where T : class
    {
        bool isCasted = obj.TryCast(out castedValue);
        errorMessage = isCasted ? null : $"Specified value is not a {typeof(T).Name}";
        return isCasted;
    }

    /// <summary>
    /// Метод для проверки того является ли объект null
    /// </summary>
    /// <param name="obj">Проверяемый объект</param>
    /// <returns>Логическое значение, отражающее является ли объект null</returns>
    [MemberNotNullWhen(false)]
    public static bool IsNull([NotNullWhen(false)] this object? obj) =>
        obj is null;

    /// <summary>
    /// Метод для проверки того не является ли объект null
    /// </summary>
    /// <param name="obj">Проверяемый объект</param>
    /// <returns>Логическое значение, отражающее не является ли объект null</returns>
    [MemberNotNullWhen(true)]
    public static bool IsNotNull([NotNullWhen(true)] this object? obj) =>
        obj is not null;
}