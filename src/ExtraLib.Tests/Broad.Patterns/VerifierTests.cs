namespace ExtraLib.Tests.Broad.Patterns;

public static class VerifierTests
{
    private const string VALID_NAME = "name";
    private const string NOT_VALID_NAME = "s";
    private const int VALID_AGE = 18;
    private const int NOT_VALID_AGE = 17;

    [Test]
    public static void Verify_ValidAgeAndName_ReturnsValidResult()
    {
        User user = new(VALID_NAME, VALID_AGE);

        var result = Verifier.Check(user);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public static void Verify_NotValidAge_ReturnsNotValidResult()
    {
        User user = new(VALID_NAME, NOT_VALID_AGE);

        var result = Verifier.Check(user);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public static void Verify_NotValidName_ReturnsNotValidResult()
    {
        User user = new(NOT_VALID_NAME, VALID_AGE);

        var result = Verifier.Check(user);

        Assert.That(result.IsValid, Is.False);
    }
}

public class User
{
    public User(string name, int age) => (Name, Age) = (name, age);

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Name { get; private init; }

    [Required]
    [System.ComponentModel.DataAnnotations.Range(18, 45)]
    public int Age { get; private init; }
}