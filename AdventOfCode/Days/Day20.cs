namespace AdventOfCode.Days;

public class Day20 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var processor = CreateCircuit(input, out var nodes);
        var start = nodes["broadcaster"];
        

        var wtf = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            start.Receive(PulseType.Low, "");
            while (processor.Count > 0)
            {
                var node = processor.Dequeue();
                wtf.Add(node);
                nodes[node].Send();
            }
        }


        var low = nodes.Values.Select(x => x.ReceivedLowPulses).Sum();
        
        var high = nodes.Values.Select(x => x.ReceivedHighPulses).Sum();

        return (low * high).ToString();
    }

    private static Queue<string> CreateCircuit(IEnumerable<string> input, out Dictionary<string, IModule> nodes)
    {
        var processor = new Queue<string>();
        nodes = new Dictionary<string, IModule>();
        foreach (var row in input)
        {
            var split = row.Split("->");
            var connected = split[1].Split(",", StringSplitOptions.TrimEntries);
            if (row.StartsWith("broadcaster"))
            {
                nodes.Add("broadcaster", new Broadcast(connected.ToList(), nodes, "broadcaster", processor));
            }
            if (row.StartsWith("%"))
            {
                var node = split[0].Substring(1).Trim();
                nodes.Add(node, new FlipFlop(connected.ToList(), nodes, node, processor));
            }
            if (row.StartsWith("&"))
            {
                var node = split[0].Substring(1).Trim();
                nodes.Add(node, new Conjunction(connected.ToList(), nodes, node, processor));
            }
        }

        foreach (var module in nodes.Values)
        {
            if (module is not Conjunction conjunction) continue;
            
            var node = module.Node;
            var connections =nodes.Values.Where(x => x.Connected.Contains(node)).Select(x => x.Node).ToList();
            conjunction.RegisterConnection(connections);
        }

        var allNodes = nodes.Values.SelectMany(x => x.Connected).Distinct().ToList();
        //Add any output nodes
        foreach (var node in allNodes)
        {
            if (!nodes.ContainsKey(node))
            {
                var output = new Output(node);
                nodes[node] = output;
            }
        }

        return processor;
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var processor = CreateCircuit(input, out var nodes);
        var start = nodes["broadcaster"];
        
        //Four conjuction boxes are connected to the final conjuction box 
        //js, qs, dt, ts
        //All have stable loop.
        List<string> conjunctionBoxes = ["js", "qs", "dt", "ts"];
        var boxLoops = new Dictionary<string, long>();
        
        for (int i = 1;; i++)
        {
            start.Receive(PulseType.Low, "");
            while (processor.Count > 0)
            {
                var node = processor.Dequeue();
                var pulseType = nodes[node].Send();

                if (!boxLoops.ContainsKey(node) && conjunctionBoxes.Contains(node) && pulseType == PulseType.High)
                {
                    boxLoops[node] = i;
                }
            }

            if (boxLoops.Count == 4)
            {
                return LowestCommonMultiple(boxLoops.Values).ToString();
            }
        }
    }

    private static long GreatestCommonDivisor(long a, long b)
    {
        if (a < b)
        {
            (b, a) = (a, b);
        }

        while (b != 0)
        {
            var (_, divisior) = Math.DivRem(a, b);
            a = b;
            b = divisior;
        }

        return a;
    }

    private static long LowestCommonMultiple(long a, long b)
    {
        return (a * b) / GreatestCommonDivisor(a, b);
    }

    private static long LowestCommonMultiple(IEnumerable<long> values)
    {
        return values.Aggregate(LowestCommonMultiple);
    }
    
    public int Day => 20;
}

public enum PulseType
{
    High,
    Low
}

public enum State
{
    On,
    Off
}

interface IModule
{
    public string Node { get; }
    public int ReceivedLowPulses { get; }
    
    public int ReceivedHighPulses { get; }
    public void Receive(PulseType pulseType, string node);

    public PulseType Send();
    
    public List<string> Connected { get; }
}

class FlipFlop(List<string> connected, IReadOnlyDictionary<string, IModule> nodes, string node, Queue<string> processor) : IModule
{
    private State State { get; set; } = State.Off;
    public string Node { get; } = node;
    public int ReceivedLowPulses { get; private set; }
    public int ReceivedHighPulses { get; private set; }


    public void Receive(PulseType pulseType, string connectedNode)
    {
        if (pulseType == PulseType.High)
        {
            ReceivedHighPulses++;
        }
        else
        {
            ReceivedLowPulses++;
        }



        switch (pulseType)
        {
            case PulseType.High:
                return;
            case PulseType.Low:
                State = State == State.Off ? State.On : State.Off;
                processor.Enqueue(Node);
     

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pulseType), pulseType, null);
        }
    }
    
    public PulseType Send()
    {
        var pulseType = State == State.On
            ? PulseType.High
            : PulseType.Low;
        foreach (var connectedNode in Connected)
        {
            nodes[connectedNode].Receive(pulseType, Node);
        }

        return pulseType;
    }
    
    
    public List<string> Connected { get; } = connected;
}

internal class Conjunction(
    List<string> connected,
    IReadOnlyDictionary<string, IModule> nodes,
    string node,
    Queue<string> processor)
    : IModule
{
    private readonly Dictionary<string, PulseType> _previousState = new ();

    public void RegisterConnection(IEnumerable<string> connectedNodes)
    {
        foreach (var connectedNode in connectedNodes)
        {
            _previousState[connectedNode] = PulseType.Low;       
        }
    }

    public string Node { get; } = node;
    public int ReceivedLowPulses { get; private set; }
    public int ReceivedHighPulses { get; private set; }
    
    public void Receive(PulseType pulseType, string connectedNode)
    {
        if (pulseType == PulseType.High)
        {
            ReceivedHighPulses++;
        }
        else
        {
            ReceivedLowPulses++;
        }
        _previousState[connectedNode] = pulseType;
         processor.Enqueue(Node);
    }
    
    public PulseType Send()
    {
        var pulseToSend = _previousState.Values.All(x => x == PulseType.High) ? PulseType.Low : PulseType.High;

        
        foreach (var connectedNode in Connected)
        {
            nodes[connectedNode].Receive(pulseToSend, Node);
        }

        return pulseToSend;
    }

    public List<string> Connected { get; } = connected;
}

internal class Output : IModule
{
    public Output(string node)
    {
        Node = node;
    }
    public string Node { get; }
    public int ReceivedLowPulses { get; private set; } = 0;
    public int ReceivedHighPulses { get;private set; } = 0;
    public void Receive(PulseType pulseType, string node)
    {
        if (pulseType == PulseType.High)
        {
            ReceivedHighPulses++;
        }
        else
        {
            ReceivedLowPulses++;
        }
    }

    public PulseType Send()
    {
        throw new NotImplementedException();
    }

    public List<string> Connected { get; }
}
internal class Broadcast(
    List<string> connected,
    IReadOnlyDictionary<string, IModule> nodes,
    string node,
    Queue<string> processor)
    : IModule
{
    public string Node { get; } = node;
    public int ReceivedLowPulses { get; private set; }
    public int ReceivedHighPulses { get; private set; }

    private PulseType _receivedPulse = PulseType.High;
    public void Receive(PulseType pulseType, string connectedNode)
    {
        if (pulseType == PulseType.High)
        {
            ReceivedHighPulses++;
        }
        else
        {
            ReceivedLowPulses++;
        }
        
        _receivedPulse = pulseType;
        processor.Enqueue(Node);
    }

    public PulseType Send()
    {
        foreach (var connectedNode in Connected)
        {
            nodes[connectedNode].Receive(_receivedPulse, Node);
        }

        return _receivedPulse;
    }

    public List<string> Connected { get; } = connected;
}