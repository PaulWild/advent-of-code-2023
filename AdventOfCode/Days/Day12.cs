namespace AdventOfCode.Days;

public class ConditionRecord
{
    private readonly List<int> _springConditions;
    private readonly char[] _conditionArray;
    
    public ConditionRecord(string inputRow, int part = 1)
    {
        var repeat = part == 1 ? 1 : 5;
        var split = inputRow.Split(' ');
        var numbers = split[1].Split(',').Select(x => Convert.ToInt32(x)).ToList();
        _springConditions = Enumerable.Repeat(numbers, repeat).SelectMany(x => x).ToList();
        _conditionArray = string.Join('?', Enumerable.Repeat(split[0], repeat)).ToCharArray();
    }

    private bool CanPlaceAt(int index, int hashLength)
    {
        if (index + hashLength == _conditionArray.Length)
        {
            return _conditionArray.Skip(index).Take(hashLength).All(x => x != '.');
        }
        if (index + hashLength > _conditionArray.Length)
        {
            return false; 
        }

        return _conditionArray.Skip(index).Take(hashLength).All(x => x != '.') 
               && _conditionArray[index + hashLength] != '#';
    }
    

    public long Combinations()
    {
        var potentials = new List<(int index, long times)> { (0,1) };
        
        foreach (var springCondition in _springConditions)
        {
            var newPotentials = new List<(int index, long times)>();
            foreach (var (index, times) in potentials)
            {
                newPotentials.AddRange(FindPotentialSpots(index, springCondition, times));
            }

            potentials = newPotentials
                .GroupBy(x =>x.index)
                .Select(key =>  (key.Key, key.Select(x=>x.times).Sum()))
                .ToList();
        }

        return potentials.Where(x => _conditionArray.Skip(x.index).All(ch => ch != '#')).Select(x => x.times).Sum();

    }

    private IEnumerable<(int index, long time)> FindPotentialSpots(int index, int springCondition, long combos)
    
    {
        var potentials = new List<int>();
        while (true)
        {
            if (index >= _conditionArray.Length || index > 0 && _conditionArray[index - 1] == '#')
            {
                 break;
            }
            if (CanPlaceAt(index, springCondition))
            {
                potentials.Add(index + springCondition + 1);
            }
            index++;
        } 

        return potentials.GroupBy(x =>x).Select(key =>  (key.Key, key.Count() * combos) );

    }
}

public class Day12 : ISolution
{ 
    public string PartOne(IEnumerable<string> input)
    {
        return input
            .Select(x => new ConditionRecord(x))
            .Select(x => x.Combinations())
            .Sum()
            .ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return input
            .Select(x => new ConditionRecord(x, 2))
            .Select(x => x.Combinations())
            .Sum()
            .ToString();
    }

    public int Day => 12;
}
