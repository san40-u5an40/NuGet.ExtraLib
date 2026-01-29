namespace ExtraLib.Tests.Broad.Patterns;

public static class AsyncChainTests
{
    private const string DOCUMENT_PATH = @".\NumberForAsyncChainTests.txt";
    private const int DOCUMENT_NUMBER = 3; // Лежит в документе по указанному пути
    private const int EXPECTED = 4;
    private const string ERROR = "Не круто, братан... Вообще не круто...";

    [Test]
    public static async Task Execute_ValidChain_ReturnValidEndValue()
    {
        var asyncChainResult = await new AsyncChain<string, int, string>(DOCUMENT_PATH)
            .AddMethod<string, int>(GetNumberFromDocumentAsync)
            .AddMethod<int, int>(IncrementAsync)
            .ExecuteAsync();

        Assert.That(asyncChainResult.IsValid, Is.True);
        Assert.That(asyncChainResult.Value, Is.EqualTo(EXPECTED));
    }

    [Test]
    public static async Task Execute_NonValidChain_ReturnNonValidError()
    {
        var asyncChainResult = await new AsyncChain<string, int, string>(DOCUMENT_PATH)
            .AddMethod<string, int>(GetNumberFromDocumentAsync)
            .AddMethod<int, int>(NotValidResultIncrementAsync)
            .ExecuteAsync();

        Assert.That(asyncChainResult.IsValid, Is.False);
        Assert.That(asyncChainResult.Error, Is.EqualTo(ERROR));
    }

    [Test]
    public static async Task AddMethodWithOutParameter_FromValidChain_ReturnReadyObjectWithCurrentValue()
    {
        var asyncChainResult = await new AsyncChain<string, int, string>(DOCUMENT_PATH)
            .AddMethod<string, int>(GetNumberFromDocumentAsync, out Readyable<int> readyable)
            .AddMethod<int, int>(IncrementAsync)
            .ExecuteAsync();

        Assert.That(readyable.IsReady, Is.True);
        Assert.That(readyable.Value, Is.EqualTo(DOCUMENT_NUMBER));
    }

    private static async Task<Result<int, string>> GetNumberFromDocumentAsync(string path)
    {
        string content = await File.ReadAllTextAsync(path);
        content = content.Trim(' ', '\r', '\n');
        int contentNumber = int.Parse(content);
        return Result<int, string>.CreateSuccess(contentNumber);
    }

    private static async Task<Result<int, string>> IncrementAsync(int number) =>
        Result<int, string>.CreateSuccess(++number);
    private static async Task<Result<int, string>> NotValidResultIncrementAsync(int number) =>
        Result<int, string>.CreateFailure(ERROR);
}