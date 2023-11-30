using System.Reflection;

namespace AdventOfCode;

public interface ISolution
{
    string PartOne(IEnumerable<string> input);

    string PartTwo(IEnumerable<string> input);
    
    int Day { get; }

    string[] Input()
    {
        var padding = Day > 9 ? "" : "0";

        var filePath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? string.Empty, 
            $"Input/day_{padding}{Day}.txt");
        return File.ReadAllLines(filePath);
    }
}
