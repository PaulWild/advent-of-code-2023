using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day11 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        HashSet<(int x, int y)> map = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square == '#')
            {
                map.Add((x,y));
            }
        }

        var start = map.Min(m => m.x);
        for (var x = start+1; x < map.Max(m => m.x); x++)
        {
            if (map.Any(m => m.x==x))
                continue;

            map = map.Select(m => (m.x > x ? m.x + 1 : m.x, m.y)).ToHashSet();
            x++;
        }
        
        var starty = map.Min(m => m.y);
        for (var y = starty+1; y < map.Max(m => m.y); y++)
        {
            if (map.Any(m => m.y==y))
                continue;

            map = map.Select(m => (m.x, m.y > y ? m.y+1: m.y)).ToHashSet();
            y++;
        }

        var totalMinimumDistance = new List<int>();
        foreach (var a in map)
        {
            foreach (var b in map)
            {
                if (a == b) continue;
                totalMinimumDistance.Add(ManhattanDistance(a, b));
            }
        }
        return (totalMinimumDistance.Sum()/2).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        HashSet<(int x, int y)> map = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square == '#')
            {
                map.Add((x,y));
            }
        }

        var start = map.Min(m => m.x);
        for (var x = start+1; x < map.Max(m => m.x); x++)
        {
            if (map.Any(m => m.x==x))
                continue;

            map = map.Select(m => (m.x > x ? m.x + 999_999 : m.x, m.y)).ToHashSet();
            x+=999_999;
        }
        
        var starty = map.Min(m => m.y);
        for (var y = starty+1; y < map.Max(m => m.y); y++)
        {
            if (map.Any(m => m.y==y))
                continue;

            map = map.Select(m => (m.x, m.y > y ? m.y+999_999: m.y)).ToHashSet();
            y+=999_999;
        }

        var totalMinimumDistance = new List<long>();
        foreach (var a in map)
        {
            foreach (var b in map)
            {
                if (a == b) continue;
                totalMinimumDistance.Add(ManhattanDistance(a, b));
            }
        }
        return (totalMinimumDistance.Sum()/2).ToString();
    }


    public int Day => 11;

    private static int ManhattanDistance((int x, int y) a, (int x, int y) b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }
}
