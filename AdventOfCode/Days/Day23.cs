using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day23 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var map = ParseMap(input.ToList());
        var start = map.First(loc => loc.Key.y == 0).Key;
        var maxY = map.Keys.Select(x => x.y).Max();
        var end = map.First(loc => loc.Key.y == maxY).Key;

        var paths = GetPathCounts(start, (start.x, start.y-1), 0, map, end, (map,location) => map.DirectNeighboursWithSlopes(location));
        return paths.Max().ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var map = ParseMap(input.ToList());
        var start = map.First(loc => loc.Key.y == 0).Key;
        var maxY = map.Keys.Select(x => x.y).Max();
        var end = map.First(loc => loc.Key.y == maxY).Key;

        var nodes = FindNodes(map);
        nodes.Add(start);
        nodes.Add(end);

        Dictionary<Location, Dictionary<Location, int>> graph = new();
        
        foreach (var node in nodes)
        {
            Dictionary<Location, int> vertices = new();
            GetVertices(node, (-1,-1), map, nodes, 0,vertices);
            graph.Add(node, vertices);
        }



        return PathLengths(start, graph, new HashSet<Location>([start]), 0, end).Max().ToString();

    }

    public List<int> PathLengths(Location currentNode, Dictionary<Location, Dictionary<Location, int>> graph,
        HashSet<Location> visited, int currentPathLength, Location end)
    {
        var vertices = graph[currentNode];

        var toReturn = new List<int>();
        foreach (var vertex in vertices)
        {
            //Looped, this is not allowed
            if (visited.Contains(vertex.Key))
            {
                
            }
            //end wahoo
            else if (vertex.Key == end)
            {
                toReturn.Add(currentPathLength + vertex.Value);
            }
            else
            {
                var newVisited = visited.Select(x => x).ToHashSet();
                newVisited.Add(vertex.Key);
                toReturn.AddRange(PathLengths(vertex.Key, graph, newVisited, currentPathLength+vertex.Value, end));
            }
        }

        return toReturn;
    }

    public HashSet<Location> FindNodes(Dictionary<Location, char> map)
    {
        return map.Keys.Where(location => map.DirectNeighbours(location).Count(neighbour => map[neighbour] != '.') > 1).ToHashSet();
    }


    public void GetVertices(
        Location node, 
        Location previous, 
        Dictionary<Location, char> map, 
        HashSet<Location> nodes, 
        int currentPathLength,
        Dictionary<Location, int> vertices)
    {
        var next = map.DirectNeighbours(node).Where(x => x != previous);
        
        foreach (var newNode in next)
        {
            if (nodes.Contains(newNode))
            {
                vertices.Add(newNode, currentPathLength+1);
            }
            else
            {
                GetVertices(newNode, node, map, nodes, currentPathLength+1, vertices);
            }
        }
    }
    
    public List<int> GetPathCounts(Location currentLocation, 
        Location previousLocation, 
        int currentPathLength,
        Dictionary<Location,char> map, 
        Location end, 
        Func<Dictionary<Location,char>,Location,IEnumerable<Location>> neighbours)
    {
        var nextLocations =neighbours(map, currentLocation)
            .Where(loc => loc != previousLocation)
            .ToList();
        
        var paths = new List<int>();
        
        foreach (var nextLocation in nextLocations)
        {
            if (nextLocation == end)
            {
                paths.Add(currentPathLength+1);
                return paths;
            }

            if (nextLocations.Count == 1)
            {
                paths.AddRange(GetPathCounts(nextLocation, currentLocation, currentPathLength + 1, map, end,
                    neighbours));
            }
            else
            {
                paths.AddRange(GetPathCounts(nextLocation, currentLocation, currentPathLength + 1, map, end,
                    neighbours));
            }

        }

        return paths;
    }
    
    private static Dictionary<Location, char> ParseMap(List<string> inputList)
    {
        Dictionary<Location, char> map = new();
        foreach (var (row, y) in inputList.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square != '#')
            {
                map.Add((x, y), square);
            }
        }

        return map;
    }
    
    public int Day => 23;
}
