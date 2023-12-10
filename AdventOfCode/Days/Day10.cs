using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day10 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), char> map = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            map.Add((x, y), square);
        }

        var startingSpot = map.First(x => x.Value == 'S').Key;
        var pipes = CalculateStartingSpot(map, startingSpot);
        
        var location = startingSpot;
        var steps = 0;
        var direction = pipes.directions.First();
        var pipe = pipes.pipe;
        do
        {
            var diff = GetDiff(pipe, direction);
            direction = diff.direction;
            location = (location.x + diff.diff.dx, location.y + diff.diff.dy);
            pipe = map[location];
            steps++;

        } while (location != startingSpot);



        return (steps/2).ToString();
    }
    


    enum Direction
    {
        North,
        South,
        East,
        West
    }
    
    private static ((int dx, int dy) diff, Direction direction) GetDiff(char pipe, Direction direction)
    {
        return pipe switch
        {
            '|' when direction == Direction.North => ((0, -1), Direction.North),
            '|' when direction == Direction.South => ((0, 1), Direction.South),
            '-' when direction == Direction.East => ((1, 0), Direction.East),
            '-' when direction == Direction.West => ((-1, 0), Direction.West),
            'F' when direction == Direction.North => ((1, 0), Direction.East),
            'F' when direction == Direction.West => ((0, 1), Direction.South),
            '7' when direction == Direction.East => ((0, 1), Direction.South),
            '7' when direction == Direction.North => ((-1, 0), Direction.West),
            'L' when direction == Direction.South => ((1, 0), Direction.East),
            'L' when direction == Direction.West => ((0, -1), Direction.North),
            'J' when direction == Direction.South => ((-1, 0), Direction.West),
            'J' when direction == Direction.East => ((0, -1), Direction.North),
            _ => throw new Exception("uh-oh")
        };
    }

    private static (char pipe, List<Direction> directions) CalculateStartingSpot(Dictionary<(int x, int y), char> map, (int x, int y) startingSpot)
    {
        var maxX = map.Keys.Select(x => x.x).Max();
        var maxY = map.Keys.Select(y => y.y).Max();
        var above = startingSpot.y > 0 ? map[(startingSpot.x, startingSpot.y - 1)] : 'X';
        var below = startingSpot.y != maxY ? map[(startingSpot.x, startingSpot.y + 1)] : 'X';
        var left = startingSpot.x > 0 ? map[(startingSpot.x - 1, startingSpot.y)] : 'X';
        var right = startingSpot.x != maxX ? map[(startingSpot.x + 1, startingSpot.y)] : 'X';
        
        var validAbove = new[] { '|', 'F', '7' }.Contains(above);        
        var validBelow = new[] { '|', 'J', 'L' }.Contains(below);
        var validLeft = new[] { '-', 'F', 'L' }.Contains(left);
        var validRight = new[] { '-', 'J', '7' }.Contains(right);
        
        if (validAbove && validBelow)
        {
             return ('|', [Direction.North, Direction.South]);
        }
        if (validLeft && validRight)
        {
            return ('-', [Direction.East, Direction.West]);
        } 
        if (validAbove && validRight)
        {
            return ('L', [Direction.South, Direction.West]);
        }
        if (validAbove && validLeft)
        {
            return ('J',[Direction.South, Direction.East]);
        }
        if (validBelow && validRight)
        {
            return ('F',[Direction.North, Direction.West]);
        }
        if (validBelow && validLeft)
        {
            return ('7',[Direction.North, Direction.East]);
        }

        throw new Exception("Oops");
    }

    public string PartTwo(IEnumerable<string> input)
    {
        Dictionary<(int x, int y), char> map = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            map.Add((x, y), square);
        }

        var startingSpot = map.First(x => x.Value == 'S').Key;
        var pipes = CalculateStartingSpot(map, startingSpot);
        
        
        var location = startingSpot;
        map[startingSpot] = pipes.pipe;

        Dictionary<(int x, int y), char> loop = new(); 
        var direction = pipes.directions.First();
        var pipe = pipes.pipe;
        do
        {
            var diff = GetDiff(pipe, direction);
            direction = diff.direction;
            location = (location.x + diff.diff.dx, location.y + diff.diff.dy);
            pipe = map[location];
            loop[location] = map[location];

        } while (location != startingSpot);

        //Ray casting algorthim, in x direction 
        var xMax = map.Keys.Select(key => key.x).Max();
        var yMax = map.Keys.Select(key => key.y).Max();

        HashSet<(int x, int y)> bounded = new(); 
        
        var bound = 0;
        for (var x = 0; x < xMax; x++)
        for (var y = 0; y < yMax; y++)
        {
            if (loop.ContainsKey((x,y))) continue;

            var spots = loop
                .Where(kvp => kvp.Key.y == y && kvp.Key.x > x)
                .Where(kvp => kvp.Value != '-').OrderBy(kvp => kvp.Key.x)
                .ToList();

            var intersections = 0;
            for (var i = 0; i < spots.Count; i++)
            {
                if (spots[i].Value == '|')
                {
                    intersections++;
                    continue;
                }

                if ((spots[i].Value == 'F' && spots[i+1].Value == 'J') ||
                    (spots[i].Value == 'L' && spots[i+1].Value == '7'))
                {
                    intersections++;
              
                }

                i++;
            }
              
     
            if (intersections > 0 && intersections % 2 != 0)
            {
                bound++;
                bounded.Add((x,y));
            } 
        }
        
        return bound.ToString();
    }

    public int Day => 10;
}
