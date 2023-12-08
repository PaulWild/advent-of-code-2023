using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public class Day08 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (instructions, map) = Parse(input);

        return StepsToGoal(instructions, "AAA", map, x=> x == "ZZZ").ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (instructions, map) = Parse(input);
        
        var positions = map.Keys
            .Where(x => x.EndsWith('A'))
            .ToList();
        
        var loopLengths = positions
            .Select(t => StepsToGoal(instructions, t, map, x => x.EndsWith('Z')))
            .ToList();

        return LowestCommonMultiple(loopLengths).ToString();
    }
    
    private static (char[] instructions ,Dictionary<string, (string left , string right)> map)  Parse(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var instructions = inputList[0].ToCharArray();

        var map = inputList.Skip(2)
            .Select(row => Regex.Matches(row, "[0-9A-Z]+"))
            .ToDictionary(reg => reg[0].Value, reg => (reg[1].Value, reg[2].Value));
        return (instructions, map);
    }

    private static long StepsToGoal(char[] instructions, string startingSpot,
        IReadOnlyDictionary<string, (string left, string right)> map, Func<string, bool> reachedGoal)
    {
        var steps = 0;
        var position = map[startingSpot];

        for (;;)
        foreach (var instruction in instructions)
        {
            var next = instruction == 'L' ? position.left : position.right;
            position = map[next];
            steps++;

            if (reachedGoal(next))
            {
                return steps;
            }
        }
    }
    
    private static long GreatestCommonDivisor(long a, long b)
    {
        if (a < b)
        {
            (b, a) = (a, b);
        }

        while (b != 0)
        {
            var (_, divisior) = Math.DivRem(a, b);
            a = b;
            b = divisior;
        }

        return a;
    }

    private static long LowestCommonMultiple(long a, long b)
    {
        return (a * b) / GreatestCommonDivisor(a, b);
    }

    private static long LowestCommonMultiple(IEnumerable<long> values)
    {
        return values.Aggregate(LowestCommonMultiple);
    }

    public int Day => 08;
}
