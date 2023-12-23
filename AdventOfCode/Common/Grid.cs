global using Location = (int x, int y);
namespace AdventOfCode.Common;

public static class Grid
{
    public static IEnumerable<(int x, int y)> AllNeighbours<T>(this Dictionary<(int x, int y), T> grid,
        (int x, int y) location)
    {
        var (x, y) = location;
        
        for (var xNeighbour = x-1; xNeighbour <= x +1; xNeighbour++)
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y) && grid.TryGetValue( (xNeighbour, yNeighbour), out _))
            {
                yield return (xNeighbour, yNeighbour);
            }
        }
    }

    public static IEnumerable<(int x, int y)> DirectNeighbours<T>(this Dictionary<(int x, int y), T> grid,
        (int x, int y) location)
    {
        var (x, y) = location;
        
        if (grid.ContainsKey((x, y - 1))) yield return (x, y - 1);
        if (grid.ContainsKey((x, y + 1))) yield return (x, y + 1);
        if (grid.ContainsKey((x - 1, y))) yield return (x - 1, y);
        if (grid.ContainsKey((x + 1, y))) yield return (x + 1, y);
    }
    


    //C# % operator is not a fucking modulus operator 
    static int Mod(int x, int m) {
        return (x%m + m)%m;
    }
    public static IEnumerable<(int x, int y)> InfiniteDirectNeighbours<T>(this Dictionary<(int x, int y), T> grid,
        (int x, int y) location, int maxX, int maxY)
    {
        var x = location.x;
        var y = location.y;

        var yPlus = location.y + 1;
        var yMinus = location.y - 1;

        var xPlus = location.x + 1;
        var xMinus = location.x - 1;

        var xW = Mod(x, (maxX+1));
        var yW = Mod(y, (maxY+1));
        
        var xPlusW = Mod(xPlus,(maxX+1));
        var xMinusW = Mod(xMinus , (maxX+1));

        var yPlusW = Mod(yPlus, (maxY+1));
        var yMinusW =  Mod(yMinus, (maxY+1));
        
        // Console.WriteLine($"x: {x}, xw: {xW}, xPlus: {xPlusW}, xMinus: {xMinusW}");

        
        if (grid.ContainsKey((xW, yMinusW))) yield return (x, yMinus);
        if (grid.ContainsKey((xW, yPlusW))) yield return (x, yPlus);
        if (grid.ContainsKey((xMinusW, yW))) yield return (xMinus, y);
        if (grid.ContainsKey((xPlusW, yW))) yield return (xPlus, y);
    }

    public static IEnumerable<(int x, int y)> Neighbours2(int x, int y)
    {
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        for (var xNeighbour = x - 1; xNeighbour <= x + 1; xNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y))
            {
                yield return (xNeighbour, yNeighbour);
            }
        }
    }
    
    public static IEnumerable<(int x, int y, int z)> Neighbours3(int x, int y, int z)
    {
        for (var zNeighbour = z - 1; zNeighbour <= z + 1; zNeighbour++)
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        for (var xNeighbour = x - 1; xNeighbour <= x + 1; xNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y && zNeighbour == z))
            {
                yield return (xNeighbour, yNeighbour, zNeighbour);
            }
        }
    }

    public static IEnumerable<(int x, int y, int z, int w)> Neighbours4(int x, int y, int z, int w)
    {
        for (var wNeighbour = w - 1; wNeighbour <= w + 1; wNeighbour++)
        for (var zNeighbour = z - 1; zNeighbour <= z + 1; zNeighbour++)
        for (var yNeighbour = y - 1; yNeighbour <= y + 1; yNeighbour++)
        for (var xNeighbour = x - 1; xNeighbour <= x + 1; xNeighbour++)
        {
            if (!(xNeighbour == x && yNeighbour == y && zNeighbour == z && wNeighbour == w))
            {
                yield return (xNeighbour, yNeighbour, zNeighbour, wNeighbour);
            }
        }
    }
    public static IEnumerable<(int x, int y)> DirectNeighboursNotBackwards<T>(this Dictionary<(int x, int y), T> grid,
        (int x, int y) location, Direction currentDirection)
    {
        var (x, y) = location;
        
        
        if (grid.ContainsKey((x, y - 1)) && currentDirection != Direction.South) yield return (x, y - 1);
        if (grid.ContainsKey((x, y + 1)) && currentDirection != Direction.North) yield return (x, y + 1);
        if (grid.ContainsKey((x - 1, y)) && currentDirection != Direction.East) yield return (x - 1, y);
        if (grid.ContainsKey((x + 1, y)) && currentDirection != Direction.West) yield return (x + 1, y);
    }
    
    public enum Direction
    {
        Unknown,
        North,
        South,
        East,
        West
    }
    
    private static int ManhattanDistance(Location a, Location b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }
}