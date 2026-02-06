namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Удобные константы для примитивов
/// </summary>
public static class DefaultConstants
{
    private static Random rand = new Random();

    extension(string)
    {
        /// <summary>
        /// Непустая строка — "something"
        /// </summary>
        public static string NotEmpty => "something";
    }

    extension(int)
    {
        /// <summary>
        /// Положительное число — 1
        /// </summary>
        public static int PositiveNumber => 1;

        /// <summary>
        /// Содержит ноль (более явная форма `default`)
        /// </summary>
        public static int Zero => 0;

        /// <summary>
        /// Отрицательное число
        /// </summary>
        public static int NegativeNumber => -1;

        /// <summary>
        /// Случайное число
        /// </summary>
        public static int RandomValue => rand.Next(int.MinValue, int.MaxValue);

        /// <summary>
        /// Случайное положительное число
        /// </summary>
        public static int RandomPositiveValue => rand.Next(int.Zero, int.MaxValue);

        /// <summary>
        /// Случайное отрицательное число
        /// </summary>
        public static int RandomNegativeValue => rand.Next(int.MinValue, int.Zero);

        /// <summary>
        /// Случайное число от 0 до указанного значения
        /// </summary>
        /// <param name="max">Максимальное значение</param>
        /// <returns>Случайное число</returns>
        /// <exception cref="ArgumentOutOfRangeException">Указано отрицательное максимальное значение</exception>
        public static int Random(int max)
        {
            if (max < int.Zero)
                throw new FormatException("Не допустимо указывать отрицательное значение!");

            return rand.Next(int.Zero, max);
        }

        /// <summary>
        /// Случайное число в указанном диапазоне
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <returns>Случайное число</returns>
        public static int Random(int min, int max)
        {
            if (max < min)
                throw new FormatException("Максимальное число не может быть меньше минимального!");

            return rand.Next(min, max);
        }
    }
}