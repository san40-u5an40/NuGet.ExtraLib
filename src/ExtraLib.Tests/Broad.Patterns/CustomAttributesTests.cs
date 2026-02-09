namespace ExtraLib.Tests.Broad.Patterns;

public static class CustomAttributesTests
{
    private const string VALID_MASK = "{0}";
    private const string NOT_VALID_MASK = "{0}}";
    private const string VALID_STRING_WITH_VALUE = "TEST string";
    private const string NOT_VALID_STRING_WITH_VALUE = "string";

    [Test]
    public static void Verify_ValidObject_ReturnedValid()
    {
        ValidationObject obj = new(VALID_MASK, VALID_STRING_WITH_VALUE);
        bool expected = true;

        bool actual = Verifier.Check(obj).IsValid;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public static void Verify_NotValidMask_ReturnedNotValid()
    {
        ValidationObject obj = new(NOT_VALID_MASK, VALID_STRING_WITH_VALUE);
        bool expected = false;

        bool actual = Verifier.Check(obj).IsValid;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public static void Verify_NotValidStringWithValue_ReturnedNotValid()
    {
        ValidationObject obj = new(VALID_MASK, NOT_VALID_STRING_WITH_VALUE);
        bool expected = false;

        bool actual = Verifier.Check(obj).IsValid;

        Assert.That(actual, Is.EqualTo(expected));
    }
}

public class ValidationObject
{
    public ValidationObject(string maskForIntern, string textWithContainsValue) =>
        (MaskForIntern, TextWithContainsValue) = (maskForIntern, textWithContainsValue);

    [InternalizationSupported(1)]
    public string MaskForIntern { get; private init; }

    [Contains("TEST")]
    public string TextWithContainsValue { get; private init; }
}