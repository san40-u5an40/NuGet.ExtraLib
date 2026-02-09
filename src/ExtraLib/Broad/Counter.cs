namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Перечисление, отражающее тип операции, производимой над счётчиком
/// </summary>
public enum CounterOperationType : ushort
{
    SetValidator,
    Increment,
    Decrement,
}

/// <summary>
/// Исключение о невалидном значении счётчика
/// </summary>
public class CounterNotValidValueException : Exception
{
    public CounterNotValidValueException(Counter counter, CounterOperationType operationType, long step, string? message = null)
        : base(message) =>
        (Counter, OperationType, Step) = (counter, operationType, step);

    /// <summary>
    /// Счётчик, с которым связано исключение
    /// </summary>
    public Counter Counter { get; private init; }

    /// <summary>
    /// Операция, производимая над счётчиком
    /// </summary>
    public CounterOperationType OperationType { get; private init; }

    /// <summary>
    /// Шаг, на который изменялся счётчик
    /// </summary>
    public long Step { get; private init; }
}

/// <summary>
/// Делегат для подписки на обновления счётчика
/// </summary>
/// <param name="type">Тип операции, производимый над счётчиком</param>
/// <param name="step">Количество, на которое изменился счётчик</param>
/// <param name="counter">Источник изменений</param>
public delegate void CounterEventHandler(CounterOperationType type, long step, Counter counter);

/// <summary>
/// Счётчик
/// </summary>
public class Counter(long value = 0, string? name = null) : ICloneable, IComparable<Counter>
{
    private static int randValueForHashCode = new Random().Next(int.MinValue, int.MaxValue);
    private HashSet<CounterEventHandler> observers = [];
    private Predicate<long>? isValid = null;

    /// <summary>
    /// Значение счётчика
    /// </summary>
    public long Value => value;

    /// <summary>
    /// Имя счётчика
    /// </summary>
    public string Name => name ?? string.Empty;

    /// <summary>
    /// Установка валидатора допустимых значений
    /// </summary>
    /// <param name="isValid">Метод для определения валидности значения</param>
    public Counter SetValidator(Predicate<long> isValid)
    {
        this.isValid = isValid;

        if (!this.isValid(Value))
            throw new CounterNotValidValueException(this, CounterOperationType.SetValidator, default);

        return this;
    }

    /// <summary>
    /// Добавление и удаление обработчиков операций, осуществляемых над счётчиком
    /// </summary>
    public event CounterEventHandler Handler
    {
        add => observers.Add(value);
        remove
        {
            if (!observers.Contains(value))
                throw new ArgumentException("This delegate is not contained in the collection");
            observers.Remove(value);
        }
    }

    /// <summary>
    /// Инкрементирование счётчика со стандартным шагом
    /// </summary>
    /// <exception cref="CounterNotValidValueException">Превышение допустимого максимального значения</exception>
    public Counter Increment()
    {
        long newValue = value + 1;

        if (long.MaxValue - 1 >= value && (isValid is null ? true : isValid(newValue)))
        {
            value = newValue;
            UpdateObservers(CounterOperationType.Increment, 1);
            return this;
        }
        else
            throw new CounterNotValidValueException(this, CounterOperationType.Increment, 1);
    }

    /// <summary>
    /// Инкрементирование счётчика с указанным шагом
    /// </summary>
    /// <param name="step">Шаг увеличения счётчика</param>
    /// <exception cref="CounterNotValidValueException">Превышение допустимого максимального значения</exception>
    public Counter Increment(long step)
    {
        long newValue = value + step;

        if (long.MaxValue - step >= value && (isValid is null ? true : isValid(newValue)))
        {
            value = newValue;
            UpdateObservers(CounterOperationType.Increment, step);
            return this;
        }
        else
            throw new CounterNotValidValueException(this, CounterOperationType.Increment, step);
    }

    /// <summary>
    /// Декрементирование счётчика со стандартным шагом
    /// </summary>
    /// <exception cref="CounterNotValidValueException">Значение перешло минимально допустимый порог</exception>
    public Counter Decrement()
    {
        long newValue = value - 1;

        if (long.MinValue + 1 <= value && (isValid is null ? true : isValid(newValue)))
        {
            value = newValue;
            UpdateObservers(CounterOperationType.Decrement, 1);
            return this;
        }
        else
            throw new CounterNotValidValueException(this, CounterOperationType.Decrement, 1);
    }

    /// <summary>
    /// Декрементирование счётчика с указанным шагом
    /// </summary>
    /// <param name="step">Шаг уменьшения счётчика</param>
    /// <exception cref="CounterNotValidValueException">Значение перешло минимально допустимый порог</exception>
    public Counter Decrement(long step)
    {
        long newValue = value - step;

        if (long.MinValue + step <= value && (isValid is null ? true : isValid(newValue)))
        {
            value = newValue;
            UpdateObservers(CounterOperationType.Decrement, step);
            return this;
        }
        else
            throw new CounterNotValidValueException(this, CounterOperationType.Decrement, step);
    }

    // Вспомогательный метод для оповещения наблюдателей
    private void UpdateObservers(CounterOperationType type, long value)
    {
        foreach (CounterEventHandler action in observers)
            action(type, value, this);
    }

    /// <summary>
    /// Метод для клонирования счётчика
    /// </summary>
    /// <returns>Новый счётчик</returns>
    public object Clone() =>
        CreateCounter(value, name);

    /// <summary>
    /// Метод сравнения счётчиков
    /// </summary>
    /// <param name="counter">Счётчик для сравнения</param>
    /// <returns>Число, означающее соотношение значений счётчиков</returns>
    /// <exception cref="ArgumentNullException">В качестве значения для сравнения указан null</exception>
    public int CompareTo(Counter? counter)
    {
        if (counter is null)
            throw new ArgumentNullException(nameof(counter));

        return Value.CompareTo(counter.Value);
    }

    /// <summary>
    /// Проверка счётчика на равенство
    /// </summary>
    /// <param name="obj">Объект для сравнения</param>
    /// <returns>Логическое значение, отражающее равны ли значения объектов</returns>
    public override bool Equals(object? obj)
    {
        var counter = obj as Counter;
        if (counter is null)
            return false;

        return (Value, Name) == (counter.Value, counter.Name);
    }

    /// <summary>
    /// Получение хеш-кода объекта
    /// </summary>
    /// <returns>Хеш-код</returns>
    public override int GetHashCode() =>
        HashCode.Combine(Value, Name, randValueForHashCode);

    /// <summary>
    /// Преобразование счётчика в строковое значение
    /// </summary>
    /// <returns>Строка с указанием имени (при наличии) и значения</returns>
    public override string ToString() =>
        $"{(name is null ? string.Empty : name + ": ")}{value}";

    /// <summary>
    /// Перегрузка оператора, увеличивающая значение счётчика на указанный шаг
    /// </summary>
    /// <param name="number">Шаг увеличения счётчика</param>
    public void operator +=(long number) =>
        Increment(number);

    /// <summary>
    /// Перегрузка оператора, уменьшающая значение счётчика на указанный шаг
    /// </summary>
    /// <param name="number">Шаг уменьшения счётчика</param>
    public void operator -=(long number) =>
        Decrement(number);

    /// <summary>
    /// Инкрементирование счётчика со стандартным шагом
    /// </summary>
    /// <param name="counter">Инкрементируемый счётчик</param>
    /// <returns>Счётчик с инкрементированным значением</returns>
    public static Counter operator ++(Counter counter) =>
        counter.Increment();

    /// <summary>
    /// Декрементирование счётчика со стандартным шагом
    /// </summary>
    /// <param name="counter">Декрементируемый счётчик</param>
    /// <returns>Счётчик с декрементированным значением</returns>
    public static Counter operator --(Counter counter) => 
        counter.Decrement();

    /// <summary>
    /// Оператор сложения счётчика с числом
    /// </summary>
    /// <param name="counter">Счётчик</param>
    /// <param name="number">Шаг увеличения</param>
    /// <returns>Увеличенный счётчик</returns>
    public static Counter operator +(Counter counter, long number) =>
        counter.Increment(number);

    /// <summary>
    /// Оператор вычитания из счётчика числа
    /// </summary>
    /// <param name="counter">Счётчик</param>
    /// <param name="number">Шаг уменьшения</param>
    /// <returns>Уменьшенный счётчик</returns>
    public static Counter operator -(Counter counter, long number) =>
        counter.Decrement(number);

    /// <summary>
    /// Оператор проверки на равенство
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее равны ли значения счётчиков</returns>
    /// <exception cref="ArgumentNullException">В параметр передано null-значение</exception>
    public static bool operator ==(Counter? counter1, Counter? counter2)
    {
        if (counter1 is null)
            throw new ArgumentNullException(nameof(counter1));

        return counter1.Equals(counter2);
    }

    /// <summary>
    /// Оператор проверки на неравенство
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее не равны ли значения счётчиков</returns>
    /// <exception cref="ArgumentNullException">В параметр передано null-значение</exception>
    public static bool operator !=(Counter? counter1, Counter? counter2)
    {
        if (counter1 is null)
            throw new ArgumentNullException(nameof(counter1));

        return !counter1.Equals(counter2);

    }

    /// <summary>
    /// Оператор для сравнения значений счётчика
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее больше ли первый счётчик по сравнению со вторым</returns>
    public static bool operator >(Counter counter1, Counter? counter2) =>
        counter1.CompareTo(counter2) == 1;

    /// <summary>
    /// Оператор для сравнения значений счётчика
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее больше ли либо равен первый счётчик по сравнению со вторым</returns>
    public static bool operator >=(Counter counter1, Counter? counter2)
    {
        int comparing = counter1.CompareTo(counter2);

        return comparing == 1 || comparing == 0;
    }

    /// <summary>
    /// Оператор для сравнения значений счётчика
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее меньше ли первый счётчик по сравнению со вторым</returns>
    public static bool operator <(Counter counter1, Counter? counter2) =>
        counter1.CompareTo(counter2) == -1;

    /// <summary>
    /// Оператор для сравнения значений счётчика
    /// </summary>
    /// <param name="counter1">Первый счётчик для сравнения</param>
    /// <param name="counter2">Второй счётчик для сравнения</param>
    /// <returns>Логическое значение, отражающее меньше ли либо равен первый счётчик по сравнению со вторым</returns>
    public static bool operator <=(Counter counter1, Counter? counter2)
    {
        int comparing = counter1.CompareTo(counter2);

        return comparing == -1 || comparing == 0;
    }

    /// <summary>
    /// Фабрика счётчиков с указанными значениями
    /// </summary>
    /// <param name="value">Значение счётчика</param>
    /// <param name="name">Имя счётчика</param>
    /// <returns>Счётчик с указанными значениями</returns>
    public static Counter CreateCounter(long value, string? name = null) =>
        new Counter(value, name);

    /// <summary>
    /// Метод создания счётчика на основе замыкания
    /// </summary>
    /// <param name="startValue">Стартовое значение счётчика</param>
    /// <returns>Функция, вызов которой возвращает значение, которое затем будет инкрементированно</returns>
    public static Func<long> CreateClosuredCounter(long startValue) =>
        () => startValue++;
}