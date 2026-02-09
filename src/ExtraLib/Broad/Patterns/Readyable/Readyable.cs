namespace san40_u5an40.ExtraLib.Broad.Patterns;

/// <summary>
/// Объект, проверяемый на готовность
/// </summary>
/// <typeparam name="TValue">Тип хранимого значения</typeparam>
/// <param name="name">Имя объекта, проверяемого на готовность</param>
public class Readyable<TValue>(string? name = null) : IReadyable<TValue>
    where TValue : notnull
{
    private readonly Lock _syncObj = new();
    private TValue? _value;

    /// <summary>
    /// Имя объекта
    /// </summary>
    public string Name => name ?? string.Empty;

    /// <summary>
    /// Значение объекта
    /// </summary>
    public TValue Value
    {
        get
        {
            lock (_syncObj)
            {
                ThrowIfNotReady();
                return _value!;
            }
        }
        set
        {
            lock (_syncObj)
            {
                ThrowIfNotWaiting();
                IsInitialized = true;
                _value = value;
            }
        }
    }

    /// <summary>
    /// Состояние объекта
    /// </summary>
    public ReadyableState State { get; private set; } = ReadyableState.Waiting;

    /// <summary>
    /// Логическое значение, отражающее инициализированы ли данные
    /// </summary>
    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// Логическое значение, отражающее находится ли объект в режиме ожидания
    /// </summary>
    public bool IsWaiting => State == ReadyableState.Waiting;

    /// <summary>
    /// Логическое значение, отражающее готов ли объект
    /// </summary>
    public bool IsReady => State == ReadyableState.Ready;

    /// <summary>
    /// Логическое значение, отражающее находится ли объект в состоянии, в котором уже никогда не будет готов
    /// </summary>
    public bool IsNeverBeReady => State == ReadyableState.NeverBeReady;

    /// <summary>
    /// Выбрасывает исключение, если объект не ожидает значение
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <exception cref="ReadyableException">Исключение, ассоциированное с объектом, проверяемым на готовность</exception>
    public void ThrowIfNotWaiting(string message = "This facility is no longer in standby mode")
    {
        if (!IsWaiting)
            throw new ReadyableException(message, State, name);
    }
    void IReadyable<TValue>.ThrowIfNotWaiting() => ThrowIfNotWaiting();          // Microsoft, добавьте реализацию сразу двух интерфейсных методов одним методом с необязательным параметром

    /// <summary>
    /// Выбрасывает исключение, если объект не находится в состоянии готовности
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <exception cref="ReadyableException">Исключение, ассоциированное с объектом, проверяемым на готовность</exception>
    public void ThrowIfNotReady(string message = "The object is not ready for use")
    {
        if (!IsReady)
            throw new ReadyableException(message, State, name);
    }
    void IReadyable<TValue>.ThrowIfNotReady() => ThrowIfNotReady();              // Microsoft, добавьте реализацию сразу двух интерфейсных методов одним методом с необязательным параметром

    /// <summary>
    /// Выбрасывает исключение, если значение объекта не инициализировано
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <exception cref="ReadyableException">Исключение, ассоциированное с объектом, проверяемым на готовность</exception>
    public void ThrowIfNotInitialized(string message = "The object has not been initialized")
    {
        if (!IsInitialized)
            throw new ReadyableException(message, State, name);
    }
    void IReadyable<TValue>.ThrowIfNotInitialized() => ThrowIfNotInitialized();  // Microsoft, добавьте реализацию сразу двух интерфейсных методов одним методом с необязательным параметром

    // Управлять состоянием объекта допустимо через интерфейсную переменную (небольшой механизм защиты от необдуманного вмешательства)
    // Приводит объект в состояние готовности
    void IReadyable<TValue>.ToReady()
    {
        lock (_syncObj)
        {
            ThrowIfNotInitialized();
            ThrowIfNotWaiting();
            State = ReadyableState.Ready;
        }
    }

    // Управлять состоянием объекта допустимо через интерфейсную переменную (небольшой механизм защиты от необдуманного вмешательства)
    // Приводит объект в состояние, в котором он уже не будет готов никогда
    void IReadyable<TValue>.ToNeverBeReady()
    {
        lock (_syncObj)
        {
            _value = default;
            ThrowIfNotWaiting();
            State = ReadyableState.NeverBeReady;
        }
    }
}