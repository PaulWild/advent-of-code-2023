using AdventOfCode.Common;
using Location = (int x, int y);

namespace AdventOfCode.Days;


public class Day17 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        return FindShortestPath(input, Expand);
    }

    public string PartTwo(IEnumerable<string> input)
    {
        return FindShortestPath(input, ExpandPartTwo);
    }

    private static string FindShortestPath(IEnumerable<string> input, Func<Node, Dictionary<Location, int>, IEnumerable<Node>> expandNodes)
    {
        var inputList = input.ToList();
        var maxX = inputList.First().Length;
        var maxY = inputList.Count;
        var map = ParseMap(inputList);
        
        Location end = (maxX-1, maxY-1);

        var openSet = new PriorityQueue<Node, int>();
        //initialize 
        openSet.EnqueueRange([
            (new Node((0,1), Grid.Direction.South, 1), map[(0,1)] + ManhattanDistance((0,1), end)),
            (new Node((1,0), Grid.Direction.East, 1), map[(1,0)] + ManhattanDistance((1,0), end))
        ]);
        
        Dictionary<Node, int> costs = new()
        {
            { new Node((0,1), Grid.Direction.South, 1),map[(0,1)]},
            { new Node((1,0), Grid.Direction.East, 1), map[(1,0)]}
        };
      
        while (openSet.Count != 0)
        {
            var node = openSet.Dequeue();
            var currentCost = costs[node];

            foreach (var newNode in expandNodes(node,map))
            {
                var newCost = map[newNode.Location] + currentCost;
                
                if (newNode.Location == end)
                {
                    return newCost.ToString();
                }
                
                if (costs.ContainsKey(newNode) && costs[newNode] > newCost)
                {
                    costs[newNode] = newCost;
                } else if (costs.TryAdd(newNode, newCost))
                {
                    openSet.Enqueue(newNode, newCost + ManhattanDistance(newNode.Location, end));
                }
            }
        }

        throw new Exception("Nope");
    }

    private static int ManhattanDistance(Location a, Location b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }
    
    private static Dictionary<Location, int> ParseMap(List<string> inputList)
    {
        Dictionary<Location, int> map = new();
        foreach (var (row, y) in inputList.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
                map.Add((x, y), Convert.ToInt32(square.ToString()));
        }

        return map;
    }
    
    public record Node(Location Location, Grid.Direction Direction, int DirectionCount);

    
    private static Grid.Direction NewDirection(Location old, Location newLocation) 
    {
        if (newLocation.x - old.x == 1)
        {
            return Grid.Direction.East;
        } 
        if (newLocation.x - old.x == -1)
        {
            return Grid.Direction.West;
        }
        if (newLocation.y - old.y == 1)
        {
            return Grid.Direction.South;
        } 
        if (newLocation.y - old.y == -1)
        {
            return Grid.Direction.North;
        }

        throw new Exception("Nope");
    }

    private static IEnumerable<Node> Expand(Node currentNode, Dictionary<Location, int> map)
    {
        var neighbours = map.DirectNeighboursNotBackwards(currentNode.Location, currentNode.Direction).ToList();

        foreach (var neighbour in neighbours)
        {
            var currentDirection = currentNode.Direction;
            var newDirection = NewDirection(currentNode.Location, neighbour);
            var directionCount = newDirection == currentDirection ? currentNode.DirectionCount + 1 : 1;
            
            //can't go this way as we need to change;
            if (directionCount > 3) 
                continue;

            yield return new Node(Location: neighbour, DirectionCount: directionCount, Direction: newDirection);
        }
    }

    private static IEnumerable<Node> ExpandPartTwo(Node currentNode, Dictionary<Location, int> map)
    {
        var neighbours = map.DirectNeighboursNotBackwards(currentNode.Location, currentNode.Direction).ToList();

        var currentCount = currentNode.DirectionCount;
        foreach (var neighbour in neighbours)
        {
            var currentDirection = currentNode.Direction;
            var newDirection = NewDirection(currentNode.Location, neighbour);
            var directionCount = newDirection == currentDirection ? currentNode.DirectionCount + 1 : 1;
           
            //can't go this way as we haven't moved 4 and this is a change in direction
            if (currentCount < 4 && directionCount == 1)
                continue;
            
            //can't go this way as we have moved 10 in one direction
            if (directionCount > 10)
                continue;

            yield return new Node(Location: neighbour, DirectionCount: directionCount, Direction: newDirection);
        }
    }

    public int Day => 17;
}