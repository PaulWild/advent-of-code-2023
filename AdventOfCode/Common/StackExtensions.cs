namespace AdventOfCode.Common;

public static class StackExtensions
{
    public static void AddDistinct<T>(this Stack<T> stack, T value)
    {
        if (!stack.Contains(value))
        {
            stack.Push(value);
        }
    }
}