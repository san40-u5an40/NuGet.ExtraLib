namespace ExtraLib.Tests.Core;

public static class IEnumerableExtensionsTests
{
    [TestCaseSource(nameof(CreateSortedAscendingCollections))]
    public static void IsSorted_SortedCollection_ReturnTrue(IEnumerable<int> collection)
    {
        bool expected = true;
        bool actual = collection.IsSorted();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(CreateSortedDescendingCollections))]
    public static void IsSortedDescending_SortedDescendingCollection_ReturnTrue(IEnumerable<int> collection)
    {
        bool expected = true;
        bool actual = collection.IsSortedDescending();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(CreateUnsortedAscendingCollections))]
    public static void IsSorted_UnsortedCollection_ReturnFalse(IEnumerable<int> collection)
    {
        bool expected = false;
        bool actual = collection.IsSorted();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual, Is.EqualTo(expected));

            actual = collection
                .Order()
                .IsSorted();
            Assert.That(actual, Is.Not.EqualTo(expected));
        }
    }

    private static IEnumerable<IEnumerable<int>> CreateSortedAscendingCollections()
    {
        var lists = new List<List<int>>
        {
            new List<int> { int.MinValue, int.MinValue, - 123, -12, 0, 0, 0, 12, 12, 12414, 1232134, int.MaxValue, int.MaxValue },
            new List<int> { },
            new List<int> { 3000 }
        };

        foreach (var list in lists)
            yield return list;
    }

    private static IEnumerable<IEnumerable<int>> CreateSortedDescendingCollections()
    {
        var lists = new List<List<int>>
        {
            new List<int> { int.MaxValue, int.MaxValue, 1232134, 12414, 12, 12, 0, 0, -12, -123, int.MinValue, int.MinValue },
            new List<int> { },
            new List<int> { 3000 }
        };

        foreach (var list in lists)
            yield return list;
    }

    private static IEnumerable<IEnumerable<int>> CreateUnsortedAscendingCollections()
    {
        yield return new List<int> { 1, 2, 1, 3, 4, 5 };
    }
}