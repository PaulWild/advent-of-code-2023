using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public partial class Day19 : ISolution
{
    public record Instrument(int x, int m, int a, int s);

    public record Instruction(string? property, string? comparison, int? value, string destination);
    
    public string PartOne(IEnumerable<string> input)
    {
        var (instruments, instructions) = ParseInput(input);

        var acceptedInstruments = from instrument in instruments
            let accepted = GradeInstrument(instrument, instructions)
            where accepted
            select instrument;

        return acceptedInstruments.Select(x => x.x + x.a + x.s + x.m).Sum().ToString();
    }

    private bool GradeInstrument(Instrument instrument, Dictionary<string, List<Instruction>> instructions)
    {
        var instructionName = "in";

        while (true)
        {
            Lol:
            foreach (var instruction in instructions[instructionName])
            {
                if (instruction.property == null)
                {
                    switch (instruction.destination)
                    {
                        case "A":
                            return true;
                        case "R":
                            return false;
                        default:
                            instructionName = instruction.destination;
                            goto Lol;
                    }
                }

                var passed = InstructionPassed(instrument, instruction);

                if (!passed) continue;

                switch (instruction.destination)
                {
                    case "A":
                        return true;
                    case "R":
                        return false;
                    default:
                        instructionName = instruction.destination;
                        goto Lol;
                }


            }
        }
    }

    private static bool InstructionPassed(Instrument instrument, Instruction instruction)
    {
        var passed = false;
        passed = instruction.comparison switch
        {
            ">" => instruction.property switch
            {
                "x" => instrument.x > instruction.value,
                "m" => instrument.m > instruction.value,
                "a" => instrument.a > instruction.value,
                "s" => instrument.s > instruction.value,
                _ => passed
            },
            "<" => instruction.property switch
            {
                "x" => instrument.x < instruction.value,
                "m" => instrument.m < instruction.value,
                "a" => instrument.a < instruction.value,
                "s" => instrument.s < instruction.value,
                _ => passed
            },
            _ => passed
        };
        return passed;
    }

    public string PartTwo(IEnumerable<string> input)
    {
        throw new NotImplementedException();
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
