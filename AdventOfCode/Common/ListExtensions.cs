namespace AdventOfCode.Common;

public static class ListExtensions
{
    public static int GetSequenceHashCode<T>(this IEnumerable<T> sequence)
    {
        var hash = new HashCode();
        foreach (var element in sequence)
        {
            hash.Add(element);
        }

        return hash.ToHashCode();
    }
    
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> sequence)
    {
        var index = 0;
        foreach (var item in sequence)
        {
            yield return (item, index);
            index++;
        }
    }
}