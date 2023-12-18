using System.Globalization;

namespace AdventOfCode.Days;

public class Day18 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var instructions = input
            .Select(row => row.Split())
            .Select(split => (split[0], Convert.ToInt32(split[1])))
            .ToList();

        return CalculateArea(instructions);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var instructions = input
            .Select(row => row.Split())
            .Select(split => split[2])
            .Select(str => (_directionMap[int.Parse(str.Substring(7,1))], Int32.Parse(str.Substring(2,5), NumberStyles.HexNumber)))
            .ToList();
        
        return CalculateArea(instructions);
    }

    private readonly Dictionary<int, string> _directionMap = new()
    {
        { 0, "R" },
        { 1, "D" },
        { 2, "L" },
        { 3, "U" }
    };

    private string CalculateArea(List<(string direcion, int number)> instructions)
    {
        var map = new Stack<Location>([]);
        var diggerMap = new Stack<Digger>(new[] { new Digger((0, 0), (1, 0), (0, -1), (1, -1)) });


        var prevDirection = "U";

        foreach (var instruction in instructions)
        {

            var digger = diggerMap.Peek();
            var newDiggerLocation = MoveDigger(digger, DigDirection(instruction.direcion, instruction.number));

            diggerMap.Push(newDiggerLocation);
                
            var vertices = prevDirection switch
            {
                "U" when instruction.direcion == "R" => digger.TopLeft,
                "U" when instruction.direcion == "L" => digger.BottomLeft,
                "D" when instruction.direcion == "R" => digger.TopRight,
                "D" when instruction.direcion == "L" => digger.BottomRight,
                "L" when instruction.direcion == "U" => digger.BottomLeft,
                "L" when instruction.direcion == "D" => digger.BottomRight,
                "R" when instruction.direcion == "U" => digger.TopLeft,
                "R" when instruction.direcion == "D" => digger.TopRight,
                _ => throw new ArgumentOutOfRangeException()
            };
  
            map.Push(vertices);
            prevDirection = instruction.direcion;
        }

        var mapList = map.ToList();
        mapList.Reverse();
        mapList.Add(mapList.First());
        
        return PolygonArea(mapList).ToString();
    }

    private Digger MoveDigger(Digger digger, (int dx, int dy) diff)
    {
        return new Digger(TopLeft: Transform(digger.TopLeft, diff), TopRight: Transform(digger.TopRight, diff),
            BottomLeft: Transform(digger.BottomLeft, diff), BottomRight: Transform(digger.BottomRight, diff));
    }
    
    private record Digger(Location TopLeft, Location TopRight, Location BottomLeft, Location BottomRight);
    
    private (int dx, int dy) DigDirection(string direction, int multiplier)
    {
        return direction switch
        {
            "U" => (0, 1 * multiplier),
            "D" => (0, -1 * multiplier),
            "L" => (-1 * multiplier, 0),
            "R" => (1 * multiplier, 0),
            _ => throw new Exception("No Direction!")
        };
    }
    
    private static Location Transform(Location current, (int dx, int dy) diff)
    {
        return (current.x + diff.dx , current.y + diff.dy);
    }
    
    //Shoelace Algorthim
    private static long PolygonArea(IReadOnlyList<Location> locations) 
    { 
        long sum1 = 0;  
        long sum2 = 0;

        for (var i = 0; i < locations.Count - 1; i++)
        {
            sum1 += locations[i].x * (long)locations[i + 1].y;
            sum2 += locations[i].y * (long)locations[i + 1].x;
        }

        var area = Math.Abs(sum1 - sum2) / 2;
        return area;
    }
    
    public int Day => 18;
}
