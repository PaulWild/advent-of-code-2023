using System.Collections.Concurrent;
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
        var numberBeaten = 0;

        for (var i = 0; i < race.time; i++)
        {
            var remainingTime = race.time - i;
            var distanceTraveled = i * remainingTime;

            if (distanceTraveled > race.distance)
            {
                numberBeaten++;
            }

        }

        return numberBeaten;
    }
    
    public int Day => 06;

    [GeneratedRegex("\\d+")]
    private static partial Regex AllNumbersRegex();
}
