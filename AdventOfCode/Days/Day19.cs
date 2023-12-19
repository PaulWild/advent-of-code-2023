using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public record Instrument(int X, int M, int A, int S);

public partial class Day19 : ISolution
{
    public record Instruction(string? Property, string? Comparison, int? Value, string Destination);
    
    public string PartOne(IEnumerable<string> input)
    {
        var (instruments, instructions) = ParseInput(input);
        var tree = BuildTree(instructions["in"], instructions);
        
        var acceptedInstruments = from instrument in instruments
            let accepted = tree.GradeInstrument(instrument)
            where accepted
            select instrument;

        return acceptedInstruments.Select(x => x.X + x.A + x.S + x.M).Sum().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var (_,instructions) = ParseInput(input);
        var tree = BuildTree(instructions["in"], instructions);
        
        return tree.Combinations(new MinMax(1, 4_000),new MinMax(1, 4_000),new MinMax(1, 4_000),new MinMax(1, 4_000))
            .Select(x => MinMaxDiff(x.x) * MinMaxDiff(x.m) * MinMaxDiff(x.a) * MinMaxDiff(x.s))
            .Sum().ToString();

    }

    private static long MinMaxDiff(MinMax val)
    {
        if (val is { Min: 0, Max: 0 })
        {
            return 0;
        }

        return val.Max - val.Min + 1;
    }

    private static Tree BuildTree(List<Instruction> instructions,
        IReadOnlyDictionary<string, List<Instruction>> instructionDictionary)
    {
        var tree = new Tree();
        var (property, comparison, value, destination) = instructions.First();

        if (property == null)
        {
            switch (destination)
            {
                case "A":
                    tree.Accepted = true;
                    return tree;
                case "R":
                    tree.Accepted = false;
                    return tree;
                default:
                    return BuildTree(instructionDictionary[destination], instructionDictionary);
            }
        }

        tree.ComparisonType = comparison;
        tree.ComparisonValue = value;
        tree.ComparisonProperty = property;

        tree.Left = destination switch
        {
            "A" => new Tree() { Accepted = true },
            "R" => new Tree() { Accepted = false },
            _ => BuildTree(instructionDictionary[destination], instructionDictionary)
        };
        tree.Right = BuildTree(instructions.Skip(1).ToList(), instructionDictionary);

        return tree;
    }
    
    public int Day => 19;
    
    private static (List<Instrument> instrument,Dictionary<string, List<Instruction>> instructions)  ParseInput(IEnumerable<string> input)
    {
        var instruments = new List<Instrument>();
        var instructions = new Dictionary<string, List<Instruction>>();
        bool parsingInstruments = false;
        
        foreach (var row in input)
        {
            if (row == "")
            {
                parsingInstruments = true;
            } else if (parsingInstruments)
            {
                var matches = Regex.Matches(row, "[0-9]+");
                instruments.Add(new Instrument(int.Parse(matches[0].Value),int.Parse(matches[1].Value),int.Parse(matches[2].Value),int.Parse(matches[3].Value)));
            }
            else
            {
                var split = row.Replace("}", "").Split("{");
                var instructionName = split[0];
                var instructionList = new List<Instruction>();
                
                var instructionStrings = split[1].Split(",");

                foreach (var instruction in instructionStrings)
                {
                    var instructionSplit = instruction.Split(":");
                    if (instructionSplit.Length == 1)
                    {
                        instructionList.Add(new Instruction(null, null, null, instructionSplit[0]));
                    }
                    else
                    {
                        var instructionMatch = InstrumentRegex().Match(instructionSplit[0]);
                        instructionList.Add(new Instruction(instructionMatch.Groups["instrumentProperty"].Value,
                            instructionMatch.Groups["condition"].Value,
                            int.Parse(instructionMatch.Groups["value"].Value), instructionSplit[1]));
                    }
                }
                instructions.Add(instructionName, instructionList);
            }

        }

        return (instruments, instructions);
    }
    
    [GeneratedRegex("(?<instrumentProperty>[a-z])+(?<condition>[><])(?<value>[0-9]+)")]
    private static partial Regex InstrumentRegex();
}

public record MinMax(int Min, int Max);


public class Tree
{
    public Tree? Left;
    public Tree? Right;
    public int? ComparisonValue;
    public string? ComparisonProperty;
    public string? ComparisonType;
    public bool? Accepted;

    private bool InstructionPassed(Instrument instrument)
    {
        return ComparisonType switch
        {
            ">" => ComparisonProperty switch
            {
                "x" => instrument.X > ComparisonValue,
                "m" => instrument.M > ComparisonValue,
                "a" => instrument.A > ComparisonValue,
                "s" => instrument.S > ComparisonValue,
                _ => false
            },
            "<" => ComparisonProperty switch
            {
                "x" => instrument.X < ComparisonValue,
                "m" => instrument.M < ComparisonValue,
                "a" => instrument.A < ComparisonValue,
                "s" => instrument.S < ComparisonValue,
                _ => false
            },
            _ => false
        };
    }

    private (MinMax left, MinMax right) NewMinMax(MinMax val, string part)
    {
        if (part != ComparisonProperty)
        {
            return (val, val);
        }
        if (val is { Max: 0, Min: 0 })
        {
            return (val, val);
        }
        
        switch (ComparisonType)
        {
            case ">":
            {
                MinMax left;
                if (val.Max <= ComparisonValue)
                {
                    left = new MinMax(0, 0);
                }
                else
                {
                    var min = val.Min > ComparisonValue ? val.Min : ComparisonValue+1;
                    var max = val.Max;
                    left = new MinMax(min!.Value, max);
                }


                MinMax right;
                if (val.Min > ComparisonValue)
                {
                    right = new MinMax(0, 0);
                }
                else
                {
                    var max = val.Max <= ComparisonValue ? val.Max : ComparisonValue;
                    var min = val.Min;
                    right = new MinMax(min, max!.Value);
                }
            
                return (left, right);
            }
            case "<":
            {
                MinMax left;
                if (val.Min >= ComparisonValue)
                {
                    left = new MinMax(0, 0);
                }
                else
                {
                    var min = val.Min;
                    var max = val.Max < ComparisonValue ? val.Max : ComparisonValue-1;
                    left = new MinMax(min, max!.Value);
                }


                MinMax right;
                if (val.Max < ComparisonValue)
                {
                    right = new MinMax(0, 0);
                }
                else
                {
                    var max = val.Max; 
                    var min = val.Min >= ComparisonValue ? val.Min : ComparisonValue;
                    right = new MinMax(min!.Value, max);
                }
            
                return (left, right);
            }
        }

        throw new Exception("nope");
    }
    
    public List<(MinMax x,MinMax m,MinMax a, MinMax s)> Combinations(MinMax x, MinMax m,MinMax a, MinMax s)
    {
        switch (Accepted)
        {
            case true:
                return [(x,m,a,s)];
            case false:
                return [];
        }

        var xs = NewMinMax(x,"x");
        var ms = NewMinMax(m, "m");
        var aas = NewMinMax(a, "a");
        var ss = NewMinMax(s, "s");

        var result = Left!.Combinations(xs.left, ms.left, aas.left, ss.left);
        result.AddRange(Right!.Combinations(xs.right, ms.right, aas.right, ss.right));
        return result;
    }
    
    public bool GradeInstrument(Instrument instrument)
    {
        switch (Accepted)
        {
            case true:
                return true;
            case false:
                return false;
            default:
            {
                var instructionPassed = InstructionPassed(instrument);
                return instructionPassed ? Left!.GradeInstrument(instrument) : Right!.GradeInstrument(instrument);
            }
        }
    }
}