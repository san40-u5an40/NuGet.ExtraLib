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

  [TestCaseSource(nameof(CreateCollectionsWith2AndMoreElements))]
  public static void Have2_Where2AndMoreElements_ReturnTrue(IEnumerable<int> collection)
  {
    bool expected = true;
    bool actual = collection.Have(2);

    Assert.That(actual, Is.EqualTo(expected));
  }

  [TestCaseSource(nameof(CreateCollectionsWith2AndLessElements))]
  public static void Have3_Where2AndLessElements_ReturnFalse(IEnumerable<int> collection)
  {
    bool expected = false;
    bool actual = collection.Have(3);

    Assert.That(actual, Is.EqualTo(expected));
  }

  private static IEnumerable<IEnumerable<int>> CreateSortedAscendingCollections()
  {
    List<List<int>> lists =
    [
      [ int.MinValue, int.MinValue, - 123, -12, 0, 0, 0, 12, 12, 12414, 1232134, int.MaxValue, int.MaxValue ],
      [ ],
      [ 3000 ]
    ];

    foreach (var list in lists)
      yield return list;
  }

  private static IEnumerable<IEnumerable<int>> CreateSortedDescendingCollections()
  {
    List<List<int>> lists =
    [
      [ int.MaxValue, int.MaxValue, 1232134, 12414, 12, 12, 0, 0, -12, -123, int.MinValue, int.MinValue ],
      [ ],
      [ 3000 ]
    ];

    foreach (var list in lists)
      yield return list;
  }

  private static IEnumerable<IEnumerable<int>> CreateUnsortedAscendingCollections()
  {
    yield return new List<int> { 1, 2, 1, 3, 4, 5 };
  }

  private static IEnumerable<IEnumerable<int>> CreateCollectionsWith2AndMoreElements()
  {
    List<List<int>> lists =
    [
      [ 1, 1 ],
      [ 1, 1, 1 ],
      [ 1, 1, 1, 1, 1, 1, 1 ]
    ];

    foreach (var list in lists)
      yield return list;
  }

  private static IEnumerable<IEnumerable<int>> CreateCollectionsWith2AndLessElements()
  {
    List<List<int>> lists =
    [
      [ ],
      [ 1 ],
      [ 1, 1 ]
    ];

    foreach (var list in lists)
      yield return list;
  }
}
