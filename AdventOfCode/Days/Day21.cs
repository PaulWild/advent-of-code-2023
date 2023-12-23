using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day21 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var (map, start) = ParseMap(input);

        HashSet<Location> locations = [start];

        for (int i = 0; i < 64; i++)
        {
            HashSet<Location> newLocations = [];
            foreach (var location in locations.Select(location => map.DirectNeighbours(location)).SelectMany(nl => nl))
            {
                newLocations.Add(location);
            }

            locations = newLocations;
        }

        return locations.Count.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var toFind = 26501365;
        // Pattern is quadratic expansion, with a periodicity of the size of the grid.
        // I spent way too long in excel. 
        var (map, start) = ParseMap(input);
        var maxX = map.Keys.Select(key => key.x).Max();
        var maxY = map.Keys.Select(key => key.y).Max();
        
        HashSet<Location> locations = [start];
        
        List<(int idx, int cnt)> cnts = new List<(int idx, int cnt)>();
        
        
        for (int i = 1; i <= 800; i++)
        {
        
            HashSet<Location> newLocations = [];
            foreach (var location in locations.Select(location => map.InfiniteDirectNeighbours(location, maxX, maxY)).SelectMany(nl => nl))
            {
                newLocations.Add(location);
            }
        
            var remainder = 26501365 % (maxX+1);
            if (i >= remainder && (i-remainder) % (maxX+1) == 0)
            {
                cnts.Add((i, newLocations.Count));
            }

            if (cnts.Count == 3)
            {
                break;
            }
        
            locations = newLocations;
       
        }
        
        var cntStrings = cnts.Select(x => $"{{{x.idx},{x.cnt}}}");
        var str = $"{{{string.Join(',', cntStrings)}}}";
        File.WriteAllLines("nums.csv",cnts.Select(x => $"{x.idx},{x.cnt}").ToList());

        //Print the first 3 values to plug it into wolfram quadratic fitter. I'm not writing 
        //my own ... yet
        File.WriteAllText("wolframNums.txt",str);

        var a = 15107 / (decimal)17161;
        var b = 28731 / (decimal)17161;
        var c = 323777 / (decimal)17161;


        return (a * toFind * toFind + b * toFind + c).ToString();
    }
    
    private static (Dictionary<Location, char> map, Location start) ParseMap(IEnumerable<string> inputList)
    {
        Location start = (0, 0);
        Dictionary<Location, char> map = new();
        foreach (var (row, y) in inputList.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            switch (square)
            {
                case '.':
                    map.Add((x, y), '.');
                    break;
                case 'S':
                    map.Add((x, y), '.');
                    start = (x, y);
                    break;
            }
        }

        return (map, start);
    }

    public int Day => 21;
}
