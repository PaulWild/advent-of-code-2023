using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public partial class Day01 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var calibrationValues = (
            from row in input 
            select DigitRegex().Matches(row) into digits 
            select digits.First().Value + digits.Last().Value into combined 
            select Convert.ToInt64(combined)
            ).ToList();

        return calibrationValues.Sum().ToString();
    }

    private readonly Dictionary<string,string> _replacements = new () {
        {"one", "1"}, 
        {"two", "2"}, 
        {"three", "3"}, 
        {"four", "4"}, 
        {"five", "5"}, 
        {"six", "6"}, 
        {"seven", "7"}, 
        {"eight", "8"}, 
        {"nine", "9"}
    };
    
    public string PartTwo(IEnumerable<string> input)
    {
        var calibrationValues = new List<long>();
        foreach (var row in input)
        {
            var digits = DigitsWithWords().Matches(row);
            var first = digits.First().Groups[1].Value;
            if (_replacements.TryGetValue(first, out var firstReplacement))
            {
                first = firstReplacement;
            }
        
            var last = digits.Last().Groups[1].Value;
            if (_replacements.TryGetValue(last, out var secondReplacement))
            {
                last = secondReplacement;
            }
            
            var combined = first + last;
            calibrationValues.Add(Convert.ToInt64(combined));
        }

        return calibrationValues.Sum().ToString();
    }

    public int Day => 01;

    [GeneratedRegex("\\d")]
    private static partial Regex DigitRegex();
    
    [GeneratedRegex("(?=(\\d|one|two|three|four|five|six|seven|eight|nine))")]
    private static partial Regex DigitsWithWords();
}
