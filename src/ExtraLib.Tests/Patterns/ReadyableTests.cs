namespace ExtraLib.Tests.Patterns;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ReadyableTests
{
    public const int TEST_VALUE = 111;
    public Readyable<int> readyable = new();

    [Test]
    public void GetValue_FromNotReadyObject_ThrowRedyableException() =>
        Assert.Throws<ReadyableException>(() => { int test = readyable.Value; });

    [Test]
    public void ThrowIfNotReady_FromNotReadyObject_ThrowRedyableException() =>
        Assert.Throws<ReadyableException>(() => { readyable.ThrowIfNotReady(); });

    [Test]
    public void ToReady_FromNotInitializedReaydyable_ThrowReadyableException() =>
        Assert.Throws<ReadyableException>(() => { ((IReadyable<int>)readyable).ToReady(); });

    [Test]
    public void ThrowIfNotWaiting_FromReadyReadyableObject_ThrowReadyableException()
    {
        readyable.Value = TEST_VALUE;
        ((IReadyable<int>)readyable).ToReady();

        Assert.Throws<ReadyableException>(() => { readyable.ThrowIfNotWaiting(); });
    }

    [Test]
    public void GetValue_FromReadyObject_ReturnValue()
    {
        readyable.Value = TEST_VALUE;
        ((IReadyable<int>)readyable).ToReady();

        Assert.That(readyable.Value, Is.EqualTo(TEST_VALUE));
    }

    [Test]
    public void GetValue_FromNeverBeReadyObject_ThrowReadyableException()
    {
        ((IReadyable<int>)readyable).ToNeverBeReady();

        Assert.Throws<ReadyableException>(() => { int returned = readyable.Value; });
    }
}