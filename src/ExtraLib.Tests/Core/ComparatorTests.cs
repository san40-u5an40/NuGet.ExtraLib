namespace ExtraLib.Tests.Core;

public record PlayerResult(string Name, int Result);

public static class ComparatorTests
{
    [TestCaseSource(nameof(CreateUnsortedCollections))]
    public static void Sorting_UnsortedCollections_ReturnSorted(PlayerResult[] playerResultArray)
    {
        Array.Sort(playerResultArray, Comparator.GetComparator<PlayerResult, int>(p => p.Result));
        bool actual = IsSorted(playerResultArray);

        Assert.That(actual, Is.True);
    }

    // Проверяет массив результатов на возрастающую упорядоченность
    private static bool IsSorted(PlayerResult[] collection)
    {
        int pre = int.MinValue;

        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].Result < pre)
                return false;
            pre = collection[i].Result;
        }

        return true;
    }

    // Источник тестовых данных
    private static IEnumerable<PlayerResult[]> CreateUnsortedCollections()
    {
        List<List<int>> collections = [
            [9, 6, 3, 2, 23, 12, 543, 13, 34],
            [],
            [10],
            [int.MaxValue, int.MinValue],
            [1, 2, -21, -23, -120, 0, -1, -1, -2, -2, 600],
            ];
        
        foreach (List<int> collection in collections)
        {
            List<PlayerResult> temp = [];
            foreach (int number in collection)
                temp.Add(new PlayerResult(string.NotEmpty, number));
            yield return temp.ToArray();
        }
    }
}