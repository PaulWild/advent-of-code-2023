namespace AdventOfCode.Common;

public class LimitedStack<T>
{
    private readonly int _size;
    private readonly List<T> _stack;

    public LimitedStack(int limit)
    {
        _size = limit;
        _stack = new List<T>(limit);
    }
    public void Push(T item)
    {
        if (_stack.Count == _size)
        {
            _stack.RemoveAt(0);
        }
        _stack.Add(item);
    }
    
    public T Pop()
    {
        var item = _stack[^1];
        _stack.RemoveAt(_stack.Count - 1);

        return item;
    }
}