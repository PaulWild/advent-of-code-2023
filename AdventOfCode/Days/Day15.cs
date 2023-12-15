using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day15 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var toReturn = (from row in input.First().Split(',')
            let currentValue = 0 
            select CalculateHash(row, currentValue)
            ).Sum();
        
        return toReturn.ToString();
    }

 
    public string PartTwo(IEnumerable<string> input)
    {
        var boxes = new List<List<(string label, int focalLength)>>();
        for (var i = 0; i < 256; i++)
        {
            boxes.Add([]);
        }
        
        foreach (var row in input.First().Split(','))
        {
            if (row.Contains('='))
            {
                var inst =row.Split('=');
                var label = inst[0];
                var focalLength = Convert.ToInt32(inst[1]);

                var hash = CalculateHash(label);

                if (boxes[hash].Any(x => x.label == label))
                {
                    var idx = boxes[hash].FindIndex(x => x.label == label);
                    boxes[hash][idx] = (label, focalLength);
                }
                else
                {
                    boxes[hash].Add((label, focalLength));
                }
            }
            else
            {
                var inst =row.Split('-');
                var label = inst[0];

                var hash = CalculateHash(label);

                if (boxes[hash].All(x => x.label != label)) continue;
                
                var idx = boxes[hash].FindIndex(x => x.label == label);
                boxes[hash].RemoveAt(idx);

            }
        }

        var toReturn = 0;
        foreach (var (box, boxIndex) in boxes.WithIndex())
        foreach (var (lens, lensIndex) in box.WithIndex())
        {
            toReturn += (boxIndex + 1) * (lensIndex + 1) * lens.focalLength;
        }
        return toReturn.ToString();
    }

    private static int CalculateHash(string row, int currentValue = 0)
    {
        var charArray = row.ToCharArray();
        foreach (var letter in charArray)
        {
            currentValue += Convert.ToInt32(letter);
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    public int Day => 15;
}
