namespace san40_u5an40.ExtraLib.Patterns;

/// <summary>
/// Счётчик
/// </summary>
public class Counter(long _value = 0, string? _name = null) : ICloneable, IComparable<Counter>
{
  private static readonly int _randValueForHashCode = new Random().Next(int.MinValue, int.MaxValue);
  private readonly Lock _lockObj = new();
  private readonly HashSet<CounterEventHandler> _observers = [];
  private Predicate<long>? _isValid = null;

  /// <summary>
  /// Значение счётчика
  /// </summary>
  public long Value => Interlocked.Read(ref _value);

  /// <summary>
  /// Имя счётчика
  /// </summary>
  public string Name
  {
    get
    {
      lock (_lockObj)
        return _name ?? string.Empty;
    }
  }

  /// <summary>
  /// Установка валидатора допустимых значений
  /// </summary>
  /// <param name="isValid">Метод для определения валидности значения</param>
  public Counter SetValidator(Predicate<long> isValid)
  {
    lock (_lockObj)
    {
      _isValid = isValid;

      if (!_isValid(Value))
        throw new CounterNotValidValueException(this, CounterOperationType.SetValidator, default);
    }

    return this;
  }

  /// <summary>
  /// Добавление и удаление обработчиков операций, осуществляемых над счётчиком
  /// </summary>
  public event CounterEventHandler Handler
  {
    add
    {
      lock (_lockObj)
      {
        if (_observers.Contains(value))
          throw new ArgumentException("the delegate is already contained in the collection");
        _observers.Add(value);
      }
    }
    remove
    {
      lock (_lockObj)
      {
        if (!_observers.Contains(value))
          throw new ArgumentException("delegate is not contained in the collection");
        _observers.Remove(value);
      }
    }
  }

  /// <summary>
  /// Инкрементирование счётчика со стандартным шагом
  /// </summary>
  /// <exception cref="CounterNotValidValueException">Превышение допустимого максимального значения</exception>
  public Counter Increment()
  {
    lock (_lockObj)
    {
      long newValue = _value + 1;

      if (long.MaxValue - 1 >= _value && (_isValid is null || _isValid(newValue)))
      {
        _value = newValue;
        UpdateObservers(CounterOperationType.Increment, 1);
      }
      else
        throw new CounterNotValidValueException(this, CounterOperationType.Increment, 1);
    }

    return this;
  }

  /// <summary>
  /// Инкрементирование счётчика с указанным шагом
  /// </summary>
  /// <param name="step">Шаг увеличения счётчика</param>
  /// <exception cref="CounterNotValidValueException">Превышение допустимого максимального значения</exception>
  public Counter Increment(long step)
  {
    lock (_lockObj)
    {
      long newValue = _value + step;

      if (long.MaxValue - step >= _value && (_isValid is null || _isValid(newValue)))
      {
        _value = newValue;
        UpdateObservers(CounterOperationType.Increment, step);
      }
      else
        throw new CounterNotValidValueException(this, CounterOperationType.Increment, step);
    }

    return this;
  }

  /// <summary>
  /// Декрементирование счётчика со стандартным шагом
  /// </summary>
  /// <exception cref="CounterNotValidValueException">Значение перешло минимально допустимый порог</exception>
  public Counter Decrement()
  {
    lock (_lockObj)
    {
      long newValue = _value - 1;

      if (long.MinValue + 1 <= _value && (_isValid is null || _isValid(newValue)))
      {
        _value = newValue;
        UpdateObservers(CounterOperationType.Decrement, 1);
      }
      else
        throw new CounterNotValidValueException(this, CounterOperationType.Decrement, 1);
    }

    return this;
  }

  /// <summary>
  /// Декрементирование счётчика с указанным шагом
  /// </summary>
  /// <param name="step">Шаг уменьшения счётчика</param>
  /// <exception cref="CounterNotValidValueException">Значение перешло минимально допустимый порог</exception>
  public Counter Decrement(long step)
  {
    lock (_lockObj)
    {
      long newValue = _value - step;

      if (long.MinValue + step <= _value && (_isValid is null || _isValid(newValue)))
      {
        _value = newValue;
        UpdateObservers(CounterOperationType.Decrement, step);
      }
      else
        throw new CounterNotValidValueException(this, CounterOperationType.Decrement, step);
    }

    return this;
  }

  // Вспомогательный метод для оповещения наблюдателей
  private void UpdateObservers(CounterOperationType type, long value)
  {
    lock (_lockObj)
      foreach (CounterEventHandler action in _observers)
        action(type, value, this);
  }

  /// <summary>
  /// Метод для клонирования счётчика
  /// </summary>
  /// <returns>Новый счётчик</returns>
  public object Clone() =>
      CreateCounter(_value, _name);

  /// <summary>
  /// Метод сравнения счётчиков
  /// </summary>
  /// <param name="counter">Счётчик для сравнения</param>
  /// <returns>Число, означающее соотношение значений счётчиков</returns>
  /// <exception cref="ArgumentNullException">В качестве значения для сравнения указан null</exception>
  public int CompareTo(Counter? counter)
  {
    lock (_lockObj)
    {
      ArgumentNullException.ThrowIfNull(counter);
      return Value.CompareTo(counter.Value);
    }
  }

  /// <summary>
  /// Проверка счётчика на равенство
  /// </summary>
  /// <param name="obj">Объект для сравнения</param>
  /// <returns>Логическое значение, отражающее равны ли значения объектов</returns>
  public override bool Equals(object? obj)
  {
    lock (_lockObj)
    {
      if (obj is not Counter counter)
        return false;
      return (Value, Name) == (counter.Value, counter.Name);
    }
  }

  /// <summary>
  /// Получение хеш-кода объекта
  /// </summary>
  /// <returns>Хеш-код</returns>
  public override int GetHashCode()
  {
    lock (_lockObj)
      return HashCode.Combine(Value, Name, _randValueForHashCode);
  }

  /// <summary>
  /// Преобразование счётчика в строковое значение
  /// </summary>
  /// <returns>Строка с указанием имени (при наличии) и значения</returns>
  public override string ToString()
  {
    lock (_lockObj)
      return (_name is not null ? _name + ": " : string.Empty) + _value;
  }

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
    ArgumentNullException.ThrowIfNull(counter1);
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
    ArgumentNullException.ThrowIfNull(counter1);
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
      new(value, name);

  /// <summary>
  /// Метод создания счётчика на основе замыкания
  /// </summary>
  /// <param name="startValue">Стартовое значение счётчика</param>
  /// <returns>Функция, вызов которой возвращает значение, которое затем будет инкрементированно</returns>
  public static Func<long> CreateClosuredCounter(long startValue) =>
      () => startValue++;
}
