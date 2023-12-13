using System.Text;

namespace AdventOfCode.Days;

public class ConditionRecord
{
    private readonly List<int> _springConditions;
    private readonly string _condition;
    private readonly char[] _conditionArray;
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
    }

    private IEnumerable<List<int>> Combinations()
    {
        return Combinations(new List<int>(), Bag);
    }

    private (bool valid, bool isFull) IsValid(List<int> gaps, bool includeNextHashes =true)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < gaps.Count; i++)
        {
            sb.Append(string.Join("", Enumerable.Repeat('.', gaps[i])));
            if (_springConditions.Count > i || (gaps.Count == i-1 && includeNextHashes))
            {
                sb.Append(string.Join("", Enumerable.Repeat('#', _springConditions[i])));
            }
        }
        

        var str = sb.ToString();

        var testArray = str.ToCharArray();

        for (int i = 0; i < str.Length; i++)
        {
            if (_conditionArray[i] == '?')
            {
                continue;
            }

            if (_conditionArray[i] == testArray[i])
            {
                continue;
            }

            return (false, false);
        }
        
        return (true, gaps.Count >=_springConditions.Count && _conditionArray.Skip(str.Length).All(x => x is '?' or '.'));
    }

    private IEnumerable<List<int>> Combinations(List<int> combos, int bag)
    {
        var (isValid, fullValid) = IsValid(combos);
        switch (bag)
        {
            case 0:
                if (isValid && fullValid)
                {
                    yield return combos;
                }
                break;
            default:
            {
                var start = combos.Count == 0 ? 0 : 1;
                
                if (isValid && fullValid)
                {
                    yield return combos;
                }
                else if (combos.Count == 0 || isValid)
                {
                    foreach (var taken in Enumerable.Range(start, bag))
                    {
                        var newCombos = combos.Select(x => x).ToList();
                        newCombos.Add(taken);

                        if (!IsValid(newCombos, false).valid)
                        {
                            break;
                        }
                        
                        var newNewCombos = Combinations(newCombos, bag - taken);
                        foreach (var newCombo in newNewCombos)
                        {
                            yield return newCombo;
                        }
                        

                    }
                }

                break;
            }
        }
    }

    public int Arrangements()
    {
        return Combinations().Count();
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
