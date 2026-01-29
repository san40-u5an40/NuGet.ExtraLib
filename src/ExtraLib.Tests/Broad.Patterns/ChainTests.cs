namespace ExtraLib.Tests.Broad.Patterns;

public static class ChainTests
{
    private const string START_VALUE = "two";
    private const int RETURNED_FROM_SECOND_METHOD = 3;
    private const string END_VALUE = "three";
    private const string ERROR = "Не круто, братан... Вообще не круто...";

    [Test]
    public static void Execute_ValidChain_ReturnValidEndValue()
    {
        var incrementResult = new Chain<string, string, string>(START_VALUE)
            .AddMethod<string, int>(ConvertStringIntoInt)
            .AddMethod<int, int>(Increment)
            .AddMethod<int, string>(ConvertIntIntoString)
            .Execute();

        Assert.That(incrementResult.IsValid, Is.True);
        Assert.That(incrementResult.Value, Is.EqualTo(END_VALUE));
    }

    [Test]
    public static void Execute_NonValidChain_ReturnNonValidError()
    {
        var incrementResult = new Chain<string, string, string>(START_VALUE)
            .AddMethod<string, int>(ConvertStringIntoInt)
            .AddMethod<int, int>(NotValidResultIncrement)  // ← Возвращает невалидный результат
            .AddMethod<int, string>(ConvertIntIntoString)
            .Execute();

        Assert.That(incrementResult.IsValid, Is.False);
        Assert.That(incrementResult.Error, Is.EqualTo(ERROR));
    }

    [Test]
    public static void AddMethodWithOutParameter_FromValidChain_ReturnReadyObjectWithCurrentValue()
    {
        var incrementResult = new Chain<string, string, string>(START_VALUE)
            .AddMethod<string, int>(ConvertStringIntoInt)
            .AddMethod<int, int>(Increment, out Readyable<int> readyable)  // ожидается int: 3
            .AddMethod<int, string>(ConvertIntIntoString)
            .Execute();

        Assert.That(readyable.IsReady, Is.True);
        Assert.That(readyable.Value, Is.EqualTo(RETURNED_FROM_SECOND_METHOD));
    }

    private static Result<int, string> ConvertStringIntoInt(string number) =>
        number switch
    {
        "one" => Result<int, string>.CreateSuccess(1),
        "two" => Result<int, string>.CreateSuccess(2),
        "three" => Result<int, string>.CreateSuccess(3),
        "four" => Result<int, string>.CreateSuccess(4),
        "five" => Result<int, string>.CreateSuccess(5),
        "six" => Result<int, string>.CreateSuccess(6),
        "seven" => Result<int, string>.CreateSuccess(7),
        "eight" => Result<int, string>.CreateSuccess(8),
        "nine" => Result<int, string>.CreateSuccess(9),
        _ => throw new ArgumentOutOfRangeException(nameof(number)),
    };

    private static Result<int, string> Increment(int number) =>
        Result<int, string>.CreateSuccess(++number);
    private static Result<int, string> NotValidResultIncrement(int number) =>
        Result<int, string>.CreateFailure(ERROR);

    private static Result<string, string> ConvertIntIntoString(int number) =>
        number switch
    {
        1 => Result<string, string>.CreateSuccess("one"),
        2 => Result<string, string>.CreateSuccess("two"),
        3 => Result<string, string>.CreateSuccess("three"),
        4 => Result<string, string>.CreateSuccess("four"),
        5 => Result<string, string>.CreateSuccess("five"),
        6 => Result<string, string>.CreateSuccess("six"),
        7 => Result<string, string>.CreateSuccess("seven"),
        8 => Result<string, string>.CreateSuccess("eight"),
        9 => Result<string, string>.CreateSuccess("nine"),
        _ => throw new ArgumentOutOfRangeException(nameof(number)),
    };
}