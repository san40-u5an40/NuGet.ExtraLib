namespace san40_u5an40.ExtraLib.Core;

/// <summary>
/// Методы расширения для перебираемых коллекций
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Метод для проверки отсортированности перебираемой коллекции по возрастанию
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="collection">Коллекция</param>
    /// <returns>Индикатор отсортированности</returns>
    public static bool IsSorted<T>(this IEnumerable<T> collection)
        where T : IComparable<T> => collection.IsSorted(true);

    /// <summary>
    /// Метод для проверки отсортированности перебираемой коллекции по убыванию
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="collection">Коллекция</param>
    /// <returns>Индикатор отсортированности</returns>
    public static bool IsSortedDescending<T>(this IEnumerable<T> collection)
        where T : IComparable<T> => collection.IsSorted(false);

    private static bool IsSorted<T>(this IEnumerable<T> collection, bool isAscending)
        where T : IComparable<T>
    {
        if (!collection.Have(2))
            return true;

        IEnumerator<T> enumerator = collection.GetEnumerator();
        int expectedCompareValue = isAscending ? -1: 1; // Равенство, то есть 0 — тоже допустим

        enumerator.MoveNext();
        T preItem = enumerator.Current;

        while (enumerator.MoveNext())
        {
            int actualCompareValue = preItem.CompareTo(enumerator.Current);

            if (actualCompareValue != 0 && actualCompareValue != expectedCompareValue)
                return false;

            preItem = enumerator.Current;
        }

        return true;
    }

    /// <summary>
    /// Метод для проверки наличия в коллекции указанного количества элементов
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="collection">Коллекция</param>
    /// <param name="minimalSize">Требуемое количество элементов</param>
    /// <returns>Индикатор наличия в коллекции указанного количества элементов</returns>
    public static bool Have<T>(this IEnumerable<T> collection, int minimalSize)
    {
        IEnumerator<T> enumerator = collection.GetEnumerator();
        int cnt = 0;

        while(enumerator.MoveNext())
            if (++cnt >= minimalSize)
                return true;

        return false;
    }
}