namespace AdventOfCode.Common;

public static class QueueExtensions
{
    public static void AddDistinct<T>(this Queue<T> stack, T value)
    {
        if (!stack.Contains(value))
        {
            stack.Enqueue(value);
        }
    }
}