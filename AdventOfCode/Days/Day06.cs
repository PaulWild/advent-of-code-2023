using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public partial class Day06 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var races = ParseInput(input.ToList());

        return CalculateWaysToBeatRaces(races);
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var races = ParseInput(input.ToList().Select(x => x.Replace(" ", "")).ToList());

        return CalculateWaysToBeatRaces(races);
    }

    private static List<(long time, long distance)> ParseInput(IList<string> input)
    {
        var times = AllNumbersRegex().Matches(input.ToList()[0]);
        var distances = AllNumbersRegex().Matches(input.ToList()[1]);

        List<(long time, long distance)> races = [];
        for (var i = 0; i < times.Count; i++)
        {
            races.Add((Convert.ToInt64(times[i].Value), Convert.ToInt64(distances[i].Value)));
        }

        return races;
    }
    
    private static string CalculateWaysToBeatRaces(List<(long time, long distance)> races)
    {
        return races.Select(CalculateNumberWaysToBeatRace)
            .Aggregate((long)1, (agg, next) => agg * next)
            .ToString();
    }

    private static long CalculateNumberWaysToBeatRace((long time, long distance) race)
    {
        var a = (long)-1;
        var b = race.time;
        var c = -(race.distance+1);

        var sqrt = Math.Sqrt((b * b) - (4 * a * c));

        var b1 = Math.Ceiling((-b + sqrt) / (2 * a));
        var b2 = Math.Floor((-b - sqrt) / (2 * a));
        var diff = (long)Math.Round(Math.Abs(b1 - b2));

        return diff+1;
    }
    
    public int Day => 06;

    [GeneratedRegex("\\d+")]
    private static partial Regex AllNumbersRegex();
}


