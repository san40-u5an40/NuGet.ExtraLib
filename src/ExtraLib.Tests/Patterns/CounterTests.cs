namespace ExtraLib.Tests.Patterns;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CounterTests
{
    private Counter counter1 = new(value: 0);
    private Counter counter2 = new(value: 20);

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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(counter1 == counter2, Is.True);
            Assert.That(counter1 >= counter2, Is.True);
            Assert.That(counter1 <= counter2, Is.True);
        }
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

    [Test]
    public void SetValidator_OnValidValue_NotThrow()
    {
        counter1 += 10;
        counter1.SetValidator(p => p == 10);
    }

    [Test]
    public void Increment_OnNotValidValue_ThrowCounterNotValidValueException()
    {
        counter1 += 10;
        counter1.SetValidator(p => p == 10);

        Assert.Throws<CounterNotValidValueException>(() => counter1++);
    }

    [Test]
    public void SetValidator_OnNotValidValue_ThrowCounterNotValidValueException()
    {
        Assert.Throws<CounterNotValidValueException>(() => counter1.SetValidator(p => p == 10));
    }

    [Test]
    public void Increment_LongMaxValue_ThrowCounterNotValidValueException()
    {
        Counter counter = new(long.MaxValue);

        Assert.Throws<CounterNotValidValueException>(() => counter++);
    }

    [Test]
    public void Decrement_LongMinValue_ThrowCounterNotValidValueException()
    {
        Counter counter = new(long.MinValue);

        Assert.Throws<CounterNotValidValueException>(() => counter--);
    }

    [Test]
    public void Increment_LongMaxValueMinusOne_NotThrow()
    {
        Counter counter = new(long.MaxValue - 1);

        Assert.DoesNotThrow(() => counter++);
    }

    [Test]
    public void Decrement_longMinValuePlusOne_NotThrow()
    {
        Counter counter = new(long.MinValue + 1);

        Assert.DoesNotThrow(() => counter--);
    }

    [Test]
    public void IncrementOnThree_LongMaxValueMinusTwo_ThrowCounterNotValidValueException()
    {
        Counter counter = new(long.MaxValue - 2);

        Assert.Throws<CounterNotValidValueException>(() => counter += 3);
    }

    [Test]
    public void DecrementOnThree_LongMinValuePlusTwo_ThrowCounterNotValidValueException()
    {
        Counter counter = new(long.MinValue + 2);

        Assert.Throws<CounterNotValidValueException>(() => counter -= 3);
    }

    [Test]
    public void IncrementOnThree_LongMaxValueMinusThree_NotThrow()
    {
        Counter counter = new(long.MaxValue - 3);

        Assert.DoesNotThrow(() => counter += 3);
    }

    [Test]
    public void DecrementOnThree_LongMinValuePlusThree_NotThrow()
    {
        Counter counter = new(long.MinValue + 3);

        Assert.DoesNotThrow(() => counter -= 3);
    }
}