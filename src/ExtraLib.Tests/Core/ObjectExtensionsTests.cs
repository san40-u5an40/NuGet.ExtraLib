namespace ExtraLib.Tests.Core;

public static class ObjectExtensionsTests
{
    [Test]
    public static void TryCast_ValidType_ReturnValid()
    {
        object obj = string.NotEmpty;
        bool expected = true;

        bool actual = obj.TryCast(out string? str);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public static void TryCast_NotValidType_ReturnNotValid()
    {
        object obj = 10;
        bool expected = false;

        bool actual = obj.TryCast(out string? str, out string? castingError);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(castingError, Is.EqualTo("Specified value is not a String"));
        }
    }
}