namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Счётчик
/// </summary>
public class Counter(double value = 0, string? name = null) : ICloneable, IComparable<Counter>
{
    private static int randValueForHashCode = new Random().Next(int.MinValue, int.MaxValue);

    /// <summary>
    /// Значение счётчика
    /// </summary>
    public double Value => value;

    /// <summary>
    /// Имя счётчика
    /// </summary>
    public string Name => name ?? string.Empty;

    /// <summary>
    /// Инкрементирование счётчика со стандартным шагом
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Превышение допустимого максимального значения</exception>
    public Counter Increment()
    {
        if (value == double.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        value++;
        return this;
    }

    /// <summary>
    /// Инкрементирование счётчика с указанным шагом
    /// </summary>
    /// <param name="step">Шаг увеличения счётчика</param>
    /// <exception cref="ArgumentOutOfRangeException">Превышение допустимого максимального значения</exception>
    public Counter Increment(double step)
    {
        if (value + step > double.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        value += step;
        return this;
    }

    /// <summary>
    /// Декрементирование счётчика со стандартным шагом
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Значение перешло минимально допустимый порог</exception>
    public Counter Decrement()
    {
        if (value == double.MinValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        value--;
        return this;
    }

    /// <summary>
    /// Декрементирование счётчика с указанным шагом
    /// </summary>
    /// <param name="step">Шаг уменьшения счётчика</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перешло минимально допустимый порог</exception>
    public Counter Decrement(double step)
    {
        if (value - step < double.MinValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        value -= step;
        return this;
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
    public void operator +=(double number) =>
        Increment(number);

    /// <summary>
    /// Перегрузка оператора, уменьшающая значение счётчика на указанный шаг
    /// </summary>
    /// <param name="number">Шаг уменьшения счётчика</param>
    public void operator -=(double number) =>
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
    public static Counter operator +(Counter counter, double number) =>
        counter.Increment(number);

    /// <summary>
    /// Оператор вычитания из счётчика числа
    /// </summary>
    /// <param name="counter">Счётчик</param>
    /// <param name="number">Шаг уменьшения</param>
    /// <returns>Уменьшенный счётчик</returns>
    public static Counter operator -(Counter counter, double number) =>
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
    public static Counter CreateCounter(double value, string? name = null) =>
        new Counter(value, name);

    /// <summary>
    /// Метод создания счётчика на основе замыкания
    /// </summary>
    /// <param name="startValue">Стартовое значение счётчика</param>
    /// <returns>Функция, вызов которой возвращает значение, которое затем будет инкрементированно</returns>
    public static Func<double> CreateClosuredCounter(double startValue) =>
        () => startValue++;
}