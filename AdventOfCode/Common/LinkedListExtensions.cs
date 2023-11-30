namespace AdventOfCode.Common;

public static class LinkedListExtensions
{
    public static List<LinkedListNode<T>> RemoveNAfter<T>(this LinkedListNode<T> item, int count)
    {
        var list = item.List;
        var items = new List<LinkedListNode<T>>();
        for (var i = 0; i < count; i++)
        {
            item = NextOrFirst(item);
            items.Add(item);
        }

        foreach (var i in items)
        {
            list?.Remove(i);
        }

        return items;
    }

    public static void AddNAfter<T>(this LinkedListNode<T> item, IEnumerable<LinkedListNode<T>> items)
    {
        var list = item.List;
        foreach (var next in items)
        {
            list?.AddAfter(item, next);
            item = next;
        }
    }

    public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> item)
    {
        return (item.Next ?? item.List?.First) ?? throw new InvalidOperationException();
    }

}