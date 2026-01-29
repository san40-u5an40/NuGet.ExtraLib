namespace ExtraLib.Tests.Broad;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CounterTests
{
    private Counter counter1 = new Counter(value: 0);
    private Counter counter2 = new Counter(value: 20);

    [Test]
    public void Increment_TestCounter_ReturnIncrementedValue()
    {
        counter1.Increment();

        Assert.That(counter1.Value, Is.EqualTo(1));
    }

    [Test]
    public void IncrementByStep_TestCounter_ReturnIncrementedValue()
    {
        counter1.Increment(10);

        Assert.That(counter1.Value, Is.EqualTo(10));
    }

    [Test]
    public void IncrementOperationPlusPlus_TestCounter_ReturnIncrementedValue()
    {
        counter1++;

        Assert.That(counter1.Value, Is.EqualTo(1));
    }

    [Test]
    public void IncrementOperationPlusEqual_TestCounter_ReturnIncrementedValue()
    {
        counter1 += 10;

        Assert.That(counter1.Value, Is.EqualTo(10));
    }

    [Test]
    public void IncrementOperationPlusNumber_TestCounter_ReturnIncrementedValue()
    {
        counter1 = counter1 + 10;

        Assert.That(counter1.Value, Is.EqualTo(10));
    }

    [Test]
    public void Decrement_TestCounter_ReturnIncrementedValue()
    {
        counter1.Decrement();

        Assert.That(counter1.Value, Is.EqualTo(-1));
    }

    [Test]
    public void DecrementByStep_TestCounter_ReturnIncrementedValue()
    {
        counter1.Decrement(10);

        Assert.That(counter1.Value, Is.EqualTo(-10));
    }

    [Test]
    public void DecrementOperationMinusMinus_TestCounter_ReturnIncrementedValue()
    {
        counter1--;

        Assert.That(counter1.Value, Is.EqualTo(-1));
    }

    [Test]
    public void DecrementOperationMinusEqual_TestCounter_ReturnIncrementedValue()
    {
        counter1 -= 10;

        Assert.That(counter1.Value, Is.EqualTo(-10));
    }

    [Test]
    public void DecrementOperationMinusNumber_TestCounter_ReturnIncrementedValue()
    {
        counter1 = counter1 - 10;

        Assert.That(counter1.Value, Is.EqualTo(-10));
    }

    [Test]
    public void Equals_ClonedCounter_IsTrue()
    {
        counter1 += 10;
        Counter cloneCounter = (Counter)counter1.Clone();

        Assert.That(counter1.Equals(cloneCounter), Is.True);
    }

    [Test]
    public void Comparison_SecondCounterGreaterFirst_IsTrue()
    {
        Assert.That(counter2 > counter1, Is.True);
    }

    [Test]
    public void Comparison_FirstCounterLessSecond_IsTrue()
    {
        Assert.That(counter1 < counter2, Is.True);
    }

    [Test]
    public void Comparison_IncrementedFirstCounterGreaterOrEqualsSecondAndLessOrEqualsSecond_IsTrue()
    {
        counter1 += 20;

        Assert.That(counter1 == counter2, Is.True);
        Assert.That(counter1 >= counter2, Is.True);
        Assert.That(counter1 <= counter2, Is.True);
    }

    [Test]
    public void UnEqualsOperator_ClonedCounter_IsTrue()
    {
        Counter cloneCounter = (Counter)counter1.Clone();
        cloneCounter++;

        Assert.That(counter1 != cloneCounter, Is.True);
    }

    [Test]
    public void IncrementedClosuredCounter_AndEndValue_AreEqual()
    {
        int startValue = 3;
        int endValue = 6;

        var count = Counter.CreateClosuredCounter(startValue);
        for (int i = 0; i < endValue - startValue; i++)
            count();

        Assert.That(count(), Is.EqualTo(endValue));
    }
}