namespace ExtraLib.Tests.Broad;

public static class StringExtensionsTests
{
    [TestCase("Очень длинная строка, где многа букав, всё сложно крч!")]
    [TestCase("Ещё одна очень длинная строка. Какая же она неприятная по длине!")]
    [TestCase("Совсем короткая строка")]
    [TestCase("")]
    public static void Reduce_LongString_ReturnReduced(string text)
    {
        int maxLength = 23;

        var actual = text.Reduce(maxLength);
        int subtracting = maxLength - text.Length;
        int remainder = subtracting > 0 ? subtracting : 0;

        Assert.That(actual.Message, Has.Length.LessThanOrEqualTo(maxLength));
        Assert.That(actual.Remainder, Is.EqualTo(remainder));
    }

    [TestCase("             Строка,       где очень много лишних пробелов       ", " Строка, где очень много лишних пробелов ")]
    [TestCase("                ", " ")]
    [TestCase("", "")]
    public static void ReplaceWhileContain_DifferentTestCases_ReturnExpected(string text, string expected)
    {
        string actual = text.ReplaceWhileContain("  ", " ");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("{0} }")]
    [TestCase("{1}")]
    [TestCase("{0} {1}")]
    [TestCase("{")]
    [TestCase("")]
    public static void IsValidForInternalization_OfNotValidMask_ReturnedNotValid(string notValidMask)
    {
        var validationResult = notValidMask.IsValidForInternalization(1);

        Assert.That(validationResult.IsValid, Is.False);
    }

    [TestCase("{0} value", 1)]
    [TestCase("{0}_{1}_{2}", 3)]
    [TestCase("value {0}", 1)]
    [TestCase("{0}", 1)]
    [TestCase("{0, -20}", 1)]
    [TestCase("{0, 20}", 1)]
    public static void IsValidForInternalization_OfValidMask_ReturnedValid(string validMask, int internedValueCount)
    {
        var validationResult = validMask.IsValidForInternalization(internedValueCount);

        Assert.That(validationResult.IsValid, Is.True);
    }
}