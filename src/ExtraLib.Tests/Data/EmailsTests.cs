namespace ExtraLib.Tests.Data;

public static class EmailsTests
{
    [TestCase("Тут нет мейлов", "Тут нет мейлов")]
    [TestCase("А вот тут есть мой email: alexandr.dev2011@gmail.com", "А вот тут есть мой email: {email}")]
    [TestCase("alexandr.dev2011@gmail.com", "{email}")]
    [TestCase("Тут нет валидных мейлов: @gmail.com", "Тут нет валидных мейлов: @gmail.com")]
    [TestCase("Как и тут:@gmail.com", "Как и тут:@gmail.com")]
    [TestCase("Как и тут alexandr.dev2011@gmail.", "Как и тут alexandr.dev2011@gmail.")]
    [TestCase("", "")]
    public static void EmailsReplace_FromTestCases_ReturnTextWithReplacedEmails(string text, string expected)
    {
        string actual = EmailsParser.Replace(text, "{email}");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public static void EmailsParse_FromText_ReturnEmailCollection()
    {
        string firstEmail = "alexandr.dev2011@gmail.com";
        string secondEmail = "vasyanchik@custom.ru";
        string textWithTwoEmails = $"По всем вопросам обращайтесь на почту: {firstEmail}.\nСюда: {secondEmail} ни в коем случае не обращайтесь!";

        List<string> emails = EmailsParser.Parse(textWithTwoEmails);

        Assert.That(emails, Has.Count.EqualTo(2));
        Assert.That(emails, Does.Contain(firstEmail));
        Assert.That(emails, Does.Contain(secondEmail));
    }

    [TestCase("alexandr.dev2011@gmail.com", true)]
    [TestCase("vasyanchik@custom.ru", true)]
    [TestCase("vasyanchik@custom.ru вместе с текстом в конце", false)]
    [TestCase("вместе с текстом в начале vasyanchik@custom.ru", false)]
    [TestCase("alexandr.dev2011@gmail.", false)]
    [TestCase("@gmail.com", false)]
    public static void IsValid_DifferentCases_ReturnExpected(string email, bool expected)
    {
        bool actual = EmailsParser.IsValid(email);

        Assert.That(actual, Is.EqualTo(expected));
    }
}