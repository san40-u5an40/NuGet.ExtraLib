namespace san40_u5an40.ExtraLib.Validation;

/// <summary>
/// Исключение, связанное с неверными параметрами атрибута валидации
/// </summary>
public class AttributeParametersException : Exception
{
    /// <summary>
    /// Создание исключения, связанного с неверными параметрами атрибута валидации
    /// </summary>
    /// <param name="parameters">Параметры атрибута</param>
    /// <param name="message">Сообщение об ошибке</param>
    public AttributeParametersException(object[] parameters, string? message = null) : base(message) =>
        Parameters = parameters;

    /// <summary>
    /// Параметры атрибута
    /// </summary>
    public object[] Parameters { get; private init; }
}