namespace AdventOfCode.Common;

public abstract record Node<TS>(TS Location);

public abstract class AStarSearch<T,TS> where T : Node<TS> where TS : notnull
{
    private static List<TS> ReconstructPath(Dictionary<T, T> cameFrom, TS end)
    {
        var path = new List<TS>() { end };
        var current = cameFrom.Keys.First(x => Equals(x.Location,  end));
        
        while (cameFrom.TryGetValue(current, out current))
        {
            path.Add(current.Location);
        }

        path.Reverse();
        return path;
    }
    
    protected abstract int H(T currentNode, TS end);

    protected abstract int G(int currentCost, T node);
    
    protected abstract IEnumerable<T> NextNodes(T node);
    
    public List<TS> Search(T start, TS end)
    {
        var openSet = new PriorityQueue<T, int>();
        openSet.Enqueue(start, 0);
        var cameFrom = new Dictionary<T, T>();
        Dictionary<T, int> costs = new()
        {
            [start] = 0
        };

        while (openSet.Count != 0)
        {
            var node = openSet.Dequeue();
            var currentCost = costs[node];

            foreach (var newNode in NextNodes(node))
            {
                var newCost = G(currentCost, newNode);

                if (Equals(node.Location, end))
                {
                    return ReconstructPath(cameFrom, end);
                }

                if ((!costs.TryGetValue(newNode, out var value) || value <= newCost) &&
                    costs.ContainsKey(newNode)) continue;
                
                costs[newNode] =  newCost;
                cameFrom[newNode] = node;
                openSet.Enqueue(newNode, newCost + H(newNode, end));
            }
        }

        throw new Exception("Could not solve search");
    }
}