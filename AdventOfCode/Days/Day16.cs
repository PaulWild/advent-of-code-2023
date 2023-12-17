using AdventOfCode.Common;
using Delta = (int dx, int dy);


namespace AdventOfCode.Days;

public class Day16 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var maxX = inputList.First().Length;
        var maxY = inputList.Count;
        var map = ParseMap(inputList);

        Stack<(Delta delta, Location location)> deltas = new([((1, 0), (-1, 0))]);

        return Traverse(deltas, maxX, maxY, map).ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var maxX = inputList.First().Length;
        var maxY = inputList.Count;
        var map = ParseMap(inputList);
        
        Stack<(Delta delta, Location location)> deltas = new();
        
        for (var y = 0; y < maxY; y++)
        {
            //Left Spots 
            deltas.Push(((1,0), (-1,y)));
            //Right Spots
            deltas.Push(((-1,0), (maxX,y)));
        }

        for (var x = 0; x < maxX; x++)
        {
            //Top Spots
            deltas.Push(((0,1), (x,-1)));
            //Bottom Spots
            deltas.Push(((0,-1), (x,maxY)));
        }

        
        return deltas.Select(start => Traverse(new Stack<(Delta delta, Location location)>([start]), maxX, maxY, map))
            .Max()
            .ToString();
    }
    
    private List<Delta> GetDelta(char reflector, Delta currentDelta)
    {
        return reflector switch
        {
            '\\' when currentDelta.dx == 1 => [(0, 1)],
            '\\' when currentDelta.dx == -1 => [(0, -1)],
            '\\' when currentDelta.dy == -1 => [(-1, 0)],
            '\\' when currentDelta.dy == 1 => [(1, 0)],
            '/' when currentDelta.dx == 1 => [(0, -1)],
            '/' when currentDelta.dx == -1 => [(0, 1)],
            '/' when currentDelta.dy == -1 => [(1, 0)],
            '/' when currentDelta.dy == 1 => [(-1, 0)],
            '|' when currentDelta.dy != 0 => [currentDelta],
            '|' => [(0, -1), (0, 1)],
            '-' when currentDelta.dx != 0 => [currentDelta],
            '-' => [(1, 0), (-1, 0)],
            _ => throw new Exception("Nope")
        };
    }


    private static Dictionary<Location, char> ParseMap(List<string> inputList)
    {
        Dictionary<Location, char> map = new();
        foreach (var (row, y) in inputList.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square != '.')
            {
                map.Add((x, y), square);
            }
        }

        return map;
    }

    private int Traverse(Stack<(Delta delta, Location location)> deltas, int maxX, int maxY, Dictionary<Location, char> map)
    {
        HashSet<(Location location, Delta delta)> visited = new();
        while (deltas.Count != 0)
        {
            var (delta, location) = deltas.Pop();
            Location next = (location.x + delta.dx, location.y + delta.dy);

            if (next.x >= maxX || next.y >= maxY || next.x < 0 || next.y < 0)
            {
                continue;
            }

            if (!visited.Add((next,delta)))
            {
                continue;
            }

            var nextDeltas = map.TryGetValue(next, out var value)
                ? GetDelta(value, delta)
                : [delta];

            foreach (var d in nextDeltas)
            {
                deltas.Push((d, next));
            }
        }

        // var visitedLocs = visited.Select(x => x.Item1).ToList();
        // for (int y = 0; y < maxY; y++)
        // {
        //     var sb = new StringBuilder();
        //     for (int x = 0; x < maxX; x++)
        //     {
        //
        //         if (map.ContainsKey((x, y)))
        //         {
        //             sb.Append(map[(x, y)]);
        //         }
        //         else if (visitedLocs.Contains((x, y)))
        //         {
        //             sb.Append('#');
        //         }
        //         else
        //         {
        //             sb.Append('.');
        //         }
        //     }
        //     Console.WriteLine(sb.ToString());
        // }

        return visited.Select(x => x.location).Distinct().Count();
    }
    
    public int Day => 16;
}
