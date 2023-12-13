using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day13 : ISolution
{
    
    public string PartOne(IEnumerable<string> input)
    {
        var pattern = new List<string>();
        var patterns = new List<Pattern>();
        foreach (var row in input)
        {
            if (row == "")
            {
                patterns.Add(new Pattern(pattern));
                pattern = new List<string>();
            }
            else
            {
                pattern.Add(row);
            }
        }
        patterns.Add(new Pattern(pattern));

        return patterns.Select(x => x.Summarize()).Sum().ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var pattern = new List<string>();
        var patterns = new List<Pattern>();
        foreach (var row in input)
        {
            if (row == "")
            {
                patterns.Add(new Pattern(pattern));
                pattern = new List<string>();
            }
            else
            {
                pattern.Add(row);
            }
        }
        patterns.Add(new Pattern(pattern));

        return patterns.Select(x => x.SummarizePart2()).Sum().ToString();
    }

    public int Day => 13;
}

public class Pattern
{
    private List<string> _pattern;
    private List<string> _transposedPattern;

    public Pattern(List<string> input)
    {
        _pattern = input;
        _transposedPattern = new List<string>();
        //create a transposed map so we can compare strings in the horizontal again
        var map = new Dictionary<(int x, int y),char>();

        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
                //Transposed!
                map.Add((x, y), square);
        }

        for (var x = 0; x <= map.Max(m => m.Key.x); x++)
        {
            var sb = new StringBuilder();
            for (var y = 0; y <= map.Max(m => m.Key.y); y++)
            {
                sb.Append(map[(x, y)]);
            }
            _transposedPattern.Add(sb.ToString());
        }
    }

    public int Summarize()
    {
        for (var i = 1; i < _transposedPattern.Count; i++)
        {
            var steps = Math.Min(_transposedPattern.Count - i, i);

            var isMirror = true;
            for (var j = 1; j <= steps; j++)
            {
                if (_transposedPattern[i - j] != _transposedPattern[i + (j-1)])
                {
                    isMirror = false;
                    break;
                }
            }

            if (isMirror)
            {
                return i;
            }
            
        }
        
        for (var i = 1; i < _pattern.Count ; i++)
        {
            var steps = Math.Min(_pattern.Count - i, i);

            var isMirror = true;
            for (var j = 1; j <= steps; j++)
            {
                if (_pattern[i - j] != _pattern[i + (j-1)])
                {
                    isMirror = false;
                    break;
                }
            }

            if (isMirror)
            {
                return i * 100;
            }
        }
        
        throw new Exception("This shouldn't have happened");
    }
    
    public int SummarizePart2()
    {
        for (var i = 1; i < _transposedPattern.Count; i++)
        {
            var steps = Math.Min(_transposedPattern.Count - i, i);

            var differences = 0;
            var isMirror = true;
            for (var j = 1; j <= steps; j++)
            {
                if (_transposedPattern[i - j] != _transposedPattern[i + (j-1)])
                {
                    var hashes1 = _transposedPattern[i - j].ToCharArray();
                    var hashes2 = _transposedPattern[i + (j-1)].ToCharArray();
                    differences += hashes1.Where((x, idx) => x != hashes2[idx]).Count();
                    

                    if (differences != 1)
                    {
                        isMirror = false;
                        break;
                    }
                }
            }

            if (isMirror && differences == 1)
            {
                return i;
            }
            
        }
        
        for (var i = 1; i < _pattern.Count ; i++)
        {
            var steps = Math.Min(_pattern.Count - i, i);

            var isMirror = true;
            var differences = 0;
            for (var j = 1; j <= steps; j++)
            {
                if (_pattern[i - j] != _pattern[i + (j-1)])
                {
                    var hashes1 = _pattern[i - j].ToCharArray();
                    var hashes2 = _pattern[i + (j-1)].ToCharArray();
                    differences += hashes1.Where((x, idx) => x != hashes2[idx]).Count();


                    if (differences != 1)
                    {
                        isMirror = false;
                        break;
                    }
                }
            }

            if (isMirror && differences == 1)
            {
                return i * 100;
            }
        }
        
        throw new Exception("This shouldn't have happened");
    }
}

