namespace san40_u5an40.ExtraLib.Broad;

/// <summary>
/// Удобные константы для примитивов
/// </summary>
public static class DefaultConstants
{
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
        public static int RandomValue => new Random().Next(int.MinValue, int.MaxValue);

        /// <summary>
        /// Случайное положительное число
        /// </summary>
        public static int RandomPositiveValue => new Random().Next(int.Zero, int.MaxValue);

        /// <summary>
        /// Случайное отрицательное число
        /// </summary>
        public static int RandomNegativeValue => new Random().Next(int.MinValue, int.Zero);
    }
}