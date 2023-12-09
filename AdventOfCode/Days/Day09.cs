namespace AdventOfCode.Days;

public class Day09 : ISolution
{
    public List<List<long>> CalculatePyramid(List<long> input)
    {
        var toReturn = new List<List<long>> { input };
        
        while (toReturn.Last().Any(x => x != 0))
        {
            var previous = toReturn.Last();
            var next = new List<long>();
            for (var i = 0; i < previous.Count - 1; i++)
            {
                next.Add(previous[i+1]-previous[i]);
            }
            toReturn.Add(next);
        }

        return toReturn;
    }

    private static long Extrapolate(List<List<long>> pyramid)
    {
        pyramid.Reverse();
        return pyramid.Sum(row => row.Last());
    }
    
    private static long ExtrapolatePrevious(List<List<long>> pyramid)
    {
        pyramid.Reverse();

        return pyramid.Aggregate(0L, (current, row) => row.First() - current);
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        var values = input
            .Select(x => x.Split(" ").Select(num => Convert.ToInt64(num)).ToList())
            .ToList();

        return values
            .Select(CalculatePyramid)
            .Select(Extrapolate)
            .Sum()
            .ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var values = input
            .Select(x => x.Split(" ").Select(num => Convert.ToInt64(num)).ToList())
            .ToList();

        return values
            .Select(CalculatePyramid)
            .Select(ExtrapolatePrevious)
            .Sum()
            .ToString();
    }

    public int Day => 09;
}
