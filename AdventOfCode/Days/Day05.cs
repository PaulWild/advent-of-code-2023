using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day05 : ISolution
{
    private static (List<long> seeds, List<List<(long destination, long source, long range)>> maps) Parse(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var seeds = inputList[0]
            .Split(":")[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt64(x))
            .ToList();

        var maps = new List<List<(long source, long destination, long range)>>();

        var map = new List<(long source, long destination, long range)>();
        foreach (var row in inputList.Skip(2))
        {
            if (row == "")
            {
                maps.Add(map);
                map = new();
                continue;
            }

            //Skip the map type, don't care for part 1
            if (row.Contains("map"))
            {
                continue;
            }

            var rowNumbers = row.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
            map.Add((rowNumbers[0], rowNumbers[1], rowNumbers[2]));
        }
        //don't forget the last one
        maps.Add(map);
        return (seeds, maps);
    }

    private static long MapSeedToLocation(long seed, List<List<(long destination, long source, long range)>> maps)
    {
        var newSeedSpot = seed;
        foreach (var map in maps)
        foreach (var transform in map)
        {
            if (newSeedSpot >= transform.source && newSeedSpot < transform.source + transform.range)
            {
                // transform to new spot
                var diff = newSeedSpot - transform.source;
                newSeedSpot = transform.destination + diff;
                break;
            }
        }

        return newSeedSpot;
    }

    public string PartOne(IEnumerable<string> input)
    {
        var (seeds, maps) = Parse(input);

        var seedLocations = seeds.Select(seed => MapSeedToLocation(seed, maps)).ToList();
        
        return seedLocations.Min().ToString();
    }
    

    public string PartTwo(IEnumerable<string> input)
    {
        var (seedRanges, maps) = Parse(input);

        Stack<(long min, long max)> minMaxes = new ();
        for (int i=0; i<seedRanges.Count; i+=2)
        {
            minMaxes.Push((seedRanges[i], seedRanges[i]+seedRanges[i+1]));
        }

        List<long> potentialMins = new();
        while (minMaxes.Count > 0)
        {
            var minMax = minMaxes.Pop();
            var left = MapSeedToLocation(minMax.min, maps);
            var right = MapSeedToLocation(minMax.max, maps);

            //if the diff is the same then we have in increasing section
            if (right - left   == minMax.max - minMax.min)
            {
                potentialMins.Add(left);
            }
            // We are at a disjoint
            else if (minMax.max - minMax.min == 1)
            {
                potentialMins.Add(new [] {right, left}.Min());
            }
            //continue searching
            else
            {
                var mid = minMax.min + ((minMax.max - minMax.min) / 2);
                minMaxes.Push((minMax.min, mid));
                minMaxes.Push((mid, minMax.max));
            }
        }

        return potentialMins.Min().ToString();
    }

    public int Day => 05;
}
