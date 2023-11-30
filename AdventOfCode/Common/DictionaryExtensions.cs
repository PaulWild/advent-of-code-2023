namespace AdventOfCode.Common;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<T,TU>(this Dictionary<T,TU> dictionary, T key, TU initialValue, Func<TU, TU> addFunction) where T : notnull
    {
        var hasValue = dictionary.TryGetValue(key, out var value);

        if (hasValue)
        {
            if (value != null) dictionary[key] = addFunction(value);
        }
        else
        {
            dictionary.Add(key, initialValue);
        }
    }
}