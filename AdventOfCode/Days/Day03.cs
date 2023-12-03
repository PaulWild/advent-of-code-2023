using System.Net.WebSockets;
using System.Runtime.InteropServices.JavaScript;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day03 : ISolution
{
    private bool IsNumber(char value) => Char.IsNumber(value);
    public string PartOne(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        
        var maxX = inputList.First().Length;
        var maxY = inputList.Count();
        
        //Parse into grid
        Dictionary<(int x, int y), char> grid = new();
        foreach (var (row, x) in inputList.WithIndex()) 
        {
            foreach (var (col, y) in row.WithIndex())
            {
                grid.Add((x,y), col);
            }
        }

        List<(int number, bool isEnginePart)> numbers = [];
        
        var currentNumber = "";
        var currentNumberIsEnginePart = false;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            var currentChar = grid[(x, y)];
            if (currentChar == '.') continue;
            
            if (IsNumber(currentChar))
            {
                currentNumber += currentChar;
            }

            var nextToEnginePart =
                grid.AllNeighbours((x, y))
                    .Where(ch => grid[ch] != '.')
                    .Any(ch => !IsNumber(grid[ch]));

            if (nextToEnginePart)
            {
                currentNumberIsEnginePart = true;
            }
            

            if ((y + 1 == maxY && currentNumber != "") || (currentNumber != "" && !IsNumber(grid[(x, y + 1)])))
            {
                numbers.Add((Convert.ToInt32(currentNumber), currentNumberIsEnginePart));

                currentNumber = "";
                currentNumberIsEnginePart = false;
            }
        }
        
        return numbers.Where(n => n.isEnginePart).Select(n => n.number).Sum().ToString();
            
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        
        var maxX = inputList.First().Length;
        var maxY = inputList.Count();
        
        //Parse into grid
        Dictionary<(int x, int y), char> grid = new();
        foreach (var (row, x) in inputList.WithIndex()) 
        {
            foreach (var (col, y) in row.WithIndex())
            {
                grid.Add((x,y), col);
            }
        }
        
        Dictionary<(int x, int y), List<int>> gearsToNumbers = [];
        
        var currentNumber = "";
        var gears = new HashSet<(int x, int y)>();
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            var currentChar = grid[(x, y)];
            if (currentChar == '.') continue;
            
            if (IsNumber(currentChar))
            {
                currentNumber += currentChar;
            }
            
            foreach(var neighbour in grid.AllNeighbours((x, y)))
            {
                if (grid[neighbour] == '*')
                {
                    gears.Add(neighbour);
                }
            }

            if ((y + 1 == maxY && currentNumber != "") || (currentNumber != "" && !IsNumber(grid[(x, y + 1)])))
            {
                foreach (var gear in gears)
                {
                    if (gearsToNumbers.ContainsKey(gear))
                    {
                        gearsToNumbers[gear].Add(Convert.ToInt32(currentNumber));
                    }
                    else
                    {
                        gearsToNumbers.Add(gear, [Convert.ToInt32(currentNumber)]);
                    }
                }
                
                currentNumber = "";

                gears = [];
            }
        }

        return gearsToNumbers.Values.Where(x => x.Count == 2)
            .Select(x => x.Aggregate(1, (acc, next) => acc * next))
            .Sum()
            .ToString();
    }

    public int Day => 03;
}
