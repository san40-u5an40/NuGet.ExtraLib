namespace ExtraLib.Tests.Validation;

public static class CustomAttributesTests
{
    private const string VALID_MASK = "{0}";
    private const string NOT_VALID_MASK = "{0}}";
    private const string VALID_STRING_WITH_VALUE = "TEST string";
    private const string NOT_VALID_STRING_WITH_VALUE = "string";

    [Test]
    public static void Verify_ValidObject_ReturnedValid()
    {
        ValidationObjectWithValidAttributeParameters obj = new(VALID_MASK, VALID_STRING_WITH_VALUE);
        bool expected = true;

        bool actual = Verifier.Check(obj).IsValid;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public static void Verify_NotValidMask_ReturnedNotValid()
    {
        ValidationObjectWithValidAttributeParameters obj = new(NOT_VALID_MASK, VALID_STRING_WITH_VALUE);
        bool expected = false;

        var verifyResult = Verifier.Check(obj);
        bool actual = verifyResult.IsValid;
        string error = verifyResult
            .Error[0]
            .ErrorMessage!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(error, Is.EqualTo("Строка должна поддерживать интернирование 1 переменн(ой/ых)"));
        }
    }

    [Test]
    public static void Verify_NotValidStringWithValue_ReturnedNotValid()
    {
        ValidationObjectWithValidAttributeParameters obj = new(VALID_MASK, NOT_VALID_STRING_WITH_VALUE);
        bool expected = false;

        var verifyResult = Verifier.Check(obj);
        bool actual = verifyResult.IsValid;
        string error = verifyResult
            .Error[0]
            .ErrorMessage!;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(error, Is.EqualTo("Строка должна содержать подстроку \"TEST\""));
        }
    }

    [Test]
    public static void Verify_ObjectWithInvalidInternalizationAttributeParameter_ThrowAttributeParametersException()
    {
        ValidationObjectWithInvalidInternalizationParameter obj = new(string.NotEmpty);

        Assert.Throws<AttributeParametersException>(() => Verifier.Check(obj));
    }

    [Test]
    public static void Verify_ObjectWithInvalidContainsAttributeParameter_ThrowAttributeParametersException()
    {
        ValidationObjectWithInvalidContainsParameter obj = new(string.NotEmpty);

        Assert.Throws<AttributeParametersException>(() => Verifier.Check(obj));
    }
}

public class ValidationObjectWithValidAttributeParameters
{
    public ValidationObjectWithValidAttributeParameters(string maskForIntern, string textWithContainsValue) =>
        (MaskForIntern, TextWithContainsValue) = (maskForIntern, textWithContainsValue);

    [InternalizationSupported(1, "Строка должна поддерживать интернирование {0} переменн(ой/ых)")]
    public string MaskForIntern { get; private init; }

    [Contains("TEST", "Строка должна содержать подстроку \"{0}\"")]
    public string TextWithContainsValue { get; private init; }
}

public class ValidationObjectWithInvalidInternalizationParameter
{
    public ValidationObjectWithInvalidInternalizationParameter(string maskForIntern)  =>
        MaskForIntern = maskForIntern;

    [InternalizationSupported(0)]
    public string MaskForIntern { get; private init; }
}

public class ValidationObjectWithInvalidContainsParameter
{
    public ValidationObjectWithInvalidContainsParameter(string textWithContainsValue) =>
        TextWithContainsValue = textWithContainsValue;

    [Contains("")]
    public string TextWithContainsValue { get; private init; }
}