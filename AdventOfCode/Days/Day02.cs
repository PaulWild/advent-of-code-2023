using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day02 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        return RunFor(1, input);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        return RunFor(2, input);
    }

    private static string RunFor(int part, IEnumerable<string> input)
    {
        var result = 0;
        foreach (var (row, idx) in input.WithIndex())
        {
            var colourNumbers = row.Split(":")[1]
                .Split(";",  StringSplitOptions.TrimEntries)
                .SelectMany(x => x.Split(",", StringSplitOptions.TrimEntries));

            Dictionary<string, int> maxColours = new();
            foreach (var colourNumber in colourNumbers)
            {
                var split = colourNumber.Split(" ");
                var count = Convert.ToInt32(split[0]);
                var colour = split[1];

                if (maxColours.TryGetValue(colour, out var currentCount))
                {
                    if (count > currentCount)
                    {
                        maxColours[colour] = count;
                    }
                }
                else
                {
                    maxColours.Add(colour, count);
                }
            }

            switch (part)
            {
                case 1:
                {
                    if (maxColours["red"] <= 12 && maxColours["green"] <= 13 && maxColours["blue"] <= 14)
                    {
                        result += idx + 1;
                    }

                    break;
                }
                case 2:
                    result += maxColours.Values.Aggregate(1, (acc, next) => acc * next);
                    break;
            }
        }

        return result.ToString();
    }
    
    public int Day => 02;
}
