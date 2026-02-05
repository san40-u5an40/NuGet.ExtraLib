using StringResultForTests = san40_u5an40.ExtraLib.Broad.Patterns.Result<string, string>;

namespace ExtraLib.Tests.Broad.Patterns;

public static class ResultTests
{
    private const string STRING_NOT_VALID = "Невалидный результат!";
    private const string STRING_VALID = "Валидный результат!";

    [Test]
    public static void GetValue_FromNonValidResult_ThrowInvalidOperationException()
    {
        var result = StringResultForTests.CreateFailure(string.Empty);

        Assert.Throws<InvalidOperationException>(() => { string returned = result.Value; });
    }

    [Test]
    public static void GetError_FromValidResult_ThrowInvalidOperationException()
    {
        var result = StringResultForTests.CreateSuccess(string.Empty);

        Assert.Throws<InvalidOperationException>(() => { string returned = result.Error; });
    }

    [Test]
    public static void GetIsValid_FromValidResult_ReturnTrue()
    {
        var result = StringResultForTests.CreateSuccess(string.Empty);
        bool actual = result.IsValid;

        Assert.That(actual, Is.True);
    }

    [Test]
    public static void GetIsValid_FromNonValidResult_ReturnFalse()
    {
        var result = StringResultForTests.CreateFailure(string.Empty);
        bool actual = result.IsValid;

        Assert.That(actual, Is.False);
    }

    [Test]
    public static void GetValue_FromValidResult_ReturnValue()
    {
        var result = StringResultForTests.CreateSuccess(STRING_VALID);
        string actual = result.Value;

        Assert.That(actual, Is.EqualTo(STRING_VALID));
    }

    [Test]
    public static void GetError_FromNonValidResult_ReturnError()
    {
        var result = StringResultForTests.CreateFailure(STRING_NOT_VALID);
        string actual = result.Error;

        Assert.That(actual, Is.EqualTo(STRING_NOT_VALID));
    }

    [Test]
    public static void IsReady_FromSuccessResultReadyableObject_ReturnTrue()
    {
        Readyable<string> readyable = new();

        StringResultForTests.CreateSuccess(STRING_VALID, readyable);

        Assert.That(readyable.IsReady, Is.True);
    }

    [Test]
    public static void IsReady_FromFailureResultReadyableObject_ReturnFalse()
    {
        Readyable<string> readyable = new();

        StringResultForTests.CreateFailure(string.Empty, readyable);

        Assert.That(readyable.IsReady, Is.False);
    }

    [Test]
    public static void GetValue_FromFailureResultReadyableObject_ThrowReadyableException()
    {
        Readyable<string> readyable = new();

        StringResultForTests.CreateFailure(string.Empty, readyable);

        Assert.Throws<ReadyableException>(() => { var value = readyable.Value; });
    }

    [Test]
    public static void GetValue_FromSuccessResultReadyableObject_ReturnSuccessValue()
    {
        Readyable<string> readyable = new();

        StringResultForTests.CreateSuccess(STRING_VALID, readyable);

        Assert.That(readyable.Value, Is.EqualTo(STRING_VALID));
    }

    [Test]
    public static void CreateSuccessResult_WithReadyReadyable_ThrowReadyableException()
    {
        Readyable<string> readyable = new();
        readyable.Value = STRING_VALID;
        ((IReadyable<string>)readyable).ToReady();

        Assert.Throws<ReadyableException>(() => { StringResultForTests.CreateSuccess(STRING_VALID, readyable); });
    }

    [Test]
    public static void CreateFailureResult_WitNeverBeReadyReadyable_ThrowReadyableException()
    {
        Readyable<string> readyable = new();
        ((IReadyable<string>)readyable).ToNeverBeReady();

        Assert.Throws<ReadyableException>(() => { StringResultForTests.CreateFailure(STRING_NOT_VALID, readyable); });
    }
}