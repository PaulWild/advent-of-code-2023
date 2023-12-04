using System.Runtime.InteropServices.JavaScript;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day04 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var total = 0;
        foreach (var row in input)
        {
            var gameStrings = row.Split(":")[1].Split("|", StringSplitOptions.TrimEntries);
            var winningNumbers = gameStrings[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
            var gameNumbers = gameStrings[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();

            var correctNumbers = gameNumbers.Intersect(winningNumbers).ToList();

            if (correctNumbers.Count == 0)
            {
                total += 0;
            } else if (correctNumbers.Count == 1)
            {
                total += 1;
            }
            else
            {
                total += (int)Math.Pow(2, correctNumbers.Count - 1);
            }

        }
        return total.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var enumerable = input as string[] ?? input.ToArray();
        var totals = Enumerable.Repeat(1, enumerable.Length).ToArray();
        
        foreach (var (row, index) in enumerable.WithIndex())
        {
            var gameStrings = row.Split(":")[1].Split("|", StringSplitOptions.TrimEntries);
            var winningNumbers = gameStrings[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
            var gameNumbers = gameStrings[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();

            var correctNumbers = gameNumbers.Intersect(winningNumbers).ToList();
 

            for (var i = 1; i <= correctNumbers.Count; i++)
            {
                totals[i + index] += totals[index];
            }
        }
        return totals.Sum().ToString();
    }

    public int Day => 04;
}
