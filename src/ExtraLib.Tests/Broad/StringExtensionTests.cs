namespace ExtraLib.Tests.Broad;

public static class StringExtensionTests
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
}