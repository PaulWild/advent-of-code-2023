using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class ConditionRecord
{
    private readonly List<int> _springConditions;
    private readonly string _condition;
    private readonly char[] _conditionArray;
    private int _questionMarks;
    private int Bag => _condition.Length - Taken;

    private int Taken => _springConditions.Sum();

    public ConditionRecord(string inputRow, int part = 1)
    {
        var repeat = part == 1 ? 1 : 5;
        var split = inputRow.Split(' ');
        var numbers = split[1].Split(',').Select(x => Convert.ToInt32(x)).ToList();
        _springConditions = Enumerable.Repeat(numbers, repeat).SelectMany(x => x).ToList();
        _condition = string.Join('?', Enumerable.Repeat(split[0], repeat));
        _conditionArray = _condition.ToCharArray();
        _questionMarks = _conditionArray.Count(x => x == '?');
        Console.WriteLine(_questionMarks);
    }

    public IEnumerable<List<int>> Combos(List<int> current)
    {
        if (current.Count == _questionMarks)
        {
            yield return current;
        }
        else
        {

            var a = Combos(current.Append(0).ToList());
            var b = Combos(current.Append(1).ToList());

            foreach (var toReturn in a)
            {
                yield return toReturn;
            }

            foreach (var toReturn in b)
            {
                yield return toReturn;
            }
        }
    }


    private bool ValidString(string test)
    {
        if (test.Length > _condition.Length)
            return false;
        
        var testArray = test.ToCharArray();
        
        for (int i = 0; i < test.Length; i++)
        {
            if (_conditionArray[i] == '?')
            {
                continue;
            }

            if (_conditionArray[i] == testArray[i])
            {
                continue;
            }

            return false;
        }

        return true;
    }

    private IEnumerable<string> Combinations(string currentString, List<int> sections)
    {
        if (sections.Count == 0 && ValidString(currentString))
        {
            yield return currentString;
        }
        
        if (sections.Count == 0)
        {
            yield break;
        }
        
        var start = currentString.Length == 0 ? 0 : 1;

        for (int i =start; i < _condition.Length - currentString.Length; i++)
        {
            var withGap = currentString + string.Join("", Enumerable.Repeat('.', i));;
            if (ValidString(withGap))
            {
                var newString = withGap + string.Join("", Enumerable.Repeat('#', sections[0]));
                foreach (var combination in Combinations(newString, sections.Skip(1).ToList()))
                {
                    yield return combination;
                }
            }
        }
        
        
    }
    

    public int Arrangements()
    {
        var count = 0;
        foreach (var (foo, idx) in Combos([]).WithIndex())
        {
            if (idx % 10000000 ==0)
            {
                Console.WriteLine(idx);
            }
            count++;
        }

        Console.WriteLine(count);
        return count;
    }
}

public class Day12 : ISolution
{ 
    public string PartOne(IEnumerable<string> input)
    {
        return input
            .Select(x => new ConditionRecord(x, 1))
            .Select(x => x.Arrangements())
            .Sum()
            .ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return input
            .Select(x => new ConditionRecord(x, 2))
            .Select(x => x.Arrangements())
            .Sum()
            .ToString();
    }

    public int Day => 12;
}
