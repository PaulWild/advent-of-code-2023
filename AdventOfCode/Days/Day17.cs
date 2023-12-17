using AdventOfCode.Common;


namespace AdventOfCode.Days;

public record Node(Location Location, Grid.Direction Direction, int DirectionCount) : Node<Location>(Location);

public class AStarSearch(Dictionary<Location, int> costMap, int part) : AStarSearch<Node, Location>
{
    private Grid.Direction NewDirection(Location old, Location newLocation) 
    {
        return (newLocation.x - old.x,newLocation.y - old.y) switch
        {
            (1,0) => Grid.Direction.East,
            (-1,0) => Grid.Direction.West,
            (0,1) => Grid.Direction.South,
            (0,-1) => Grid.Direction.North,
            _ => throw new Exception("Can't find new direction")
        };
    }
    
    protected override IEnumerable<Node> NextNodes(Node currentNode)
    {
        
        var neighbours = costMap.DirectNeighboursNotBackwards(currentNode.Location, currentNode.Direction).ToList();

        foreach (var neighbour in neighbours)
        {
            var currentDirection = currentNode.Direction;
            var newDirection = NewDirection(currentNode.Location, neighbour);
            var directionCount = newDirection == currentDirection ? currentNode.DirectionCount + 1 : 1;

            switch (part)
            {
                //can't go this way as we need to change;
                case 1 when directionCount > 3:
                //can't go this way as we haven't moved 4 and this is a change in direction
                case 2 when currentNode.DirectionCount < 4 && directionCount == 1 && currentDirection != Grid.Direction.Unknown:
                //can't go this way as we have moved 10 in one direction
                case 2 when directionCount > 10:
                    continue;
                default:
                    yield return new Node(Location: neighbour, DirectionCount: directionCount, Direction: newDirection);
                    break;
            }
        }
    }
    
    protected override int H(Node currentNode, Location end)
    {
        return Math.Abs(end.x - currentNode.Location.x) + Math.Abs(end.y - currentNode.Location.y);
    }

    protected override int G(int currentCost, Node node)
    {
        return currentCost + costMap[node.Location];
    }
}

public class Day17 : ISolution
{
    private readonly Node _start = new Node((0, 0), Grid.Direction.Unknown, 0);
    
    public string PartOne(IEnumerable<string> input)
    {
        var inputList =  input.ToList();
        var (map, end) = ParseMap(inputList);
        var searchClass = new AStarSearch(map, 1);

        var path = searchClass.Search(_start, end);

        return path
            .Skip(1)
            .Select(x => map[x])
            .Sum()
            .ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputList =  input.ToList();
        var (map, end)= ParseMap(inputList);
        var searchClass = new AStarSearch(map, 2);
        
        var path = searchClass.Search(_start, end);

        return path
            .Skip(1)
            .Select(x => map[x])
            .Sum()
            .ToString();
    }
    
    private static (Dictionary<Location, int> map, Location end) ParseMap(List<string> input)
    {
        Dictionary<Location, int> map = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
                map.Add((x, y), Convert.ToInt32(square.ToString()));
        }

        return (map,  (input.First().Length - 1, input.Count - 1));
    }

    public int Day => 17;
}
