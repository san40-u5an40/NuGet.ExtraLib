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
        Assert.That(asyncChainResult.Error.Type, Is.EqualTo(InvalidAsyncChainResultType.NotValidMethodResult));
        Assert.That(asyncChainResult.Error.Value, Is.EqualTo(ERROR));
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

    [Test]
    public static async Task RequestCancellationToken_ForAsyncChain_ReturnInvalidResultWithCancellationTokenRequestedType()
    {
        CancellationTokenSource cancellationTokenSource = new();
        var asyncChain = new AsyncChain<string, int, string>(DOCUMENT_PATH, cancellationTokenSource.Token)
            .AddMethod<string, int>(GetNumberFromDocumentAsync)
            .AddMethod<int, int>(IncrementAsync);

        cancellationTokenSource.Cancel();
        var result = await asyncChain.ExecuteAsync();

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Error.Type, Is.EqualTo(InvalidAsyncChainResultType.CancellationTokenRequested));
        Assert.That(result.Error.Value, Is.Null);
    }

    [Test]
    public static async Task RequestCancellationToken_ForAsyncChainMethod_ReturnInvalidResultWithNotValidMethodResultType()
    {
        CancellationTokenSource cancellationTokenSource = new();
        var asyncChain = new AsyncChain<int, int, string>(int.RandomValue, cancellationTokenSource.Token)
            .AddMethod<int, int>(IncrementWithTokenAsync);

        var cancelTask = Wait5MillisecondsAndCancel(cancellationTokenSource);
        var executeTask = asyncChain.ExecuteAsync();
        await Task.WhenAll(executeTask, cancelTask);
        var result = executeTask.Result;

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Error.Type, Is.EqualTo(InvalidAsyncChainResultType.NotValidMethodResult));
        Assert.That(result.Error.Value, Is.Not.Null);
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

    private static async Task Wait5MillisecondsAndCancel(CancellationTokenSource cancellationTokenSource)
    {
        await Task.Delay(5);
        cancellationTokenSource.Cancel();
    }
    private static async Task<Result<int, string>> IncrementWithTokenAsync(int number, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Result<int, string>.CreateFailure("Операция прервана по токену");

        await Task.Delay(100);

        if (cancellationToken.IsCancellationRequested)
            return Result<int, string>.CreateFailure("Операция прервана по токену");

        return Result<int, string>.CreateSuccess(++number);
    }
}