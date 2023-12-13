using System.Text;

namespace AdventOfCode.Days;

public class Day12 : ISolution
{ 
    record InputRow(string row, List<int> hasSizes);
    public string PartOne(IEnumerable<string> input)
    {
        var rows = (from row in input 
            select row.Split(' ') into split 
            let numbers = split[1].Split(',').Select(x => Convert.ToInt32(x)).ToList() 
            select new InputRow(split[0], numbers)).ToList();

        var totalArrangements = 0;
        foreach (var row in rows)
        {
            var taken = row.hasSizes.Sum();
            var combos = Combinations(row.row.Length - taken, row.hasSizes.Count + 1);

            var foo = combos.Where(x => x.Count > row.hasSizes.Count - 2).SelectMany(x =>
            {
                if (x.Count == row.hasSizes.Count)
                {
                    return new List<List<int>>
                    {
                        x.Append(0).ToList(),
                        x.Prepend(0).ToList()
                    };
                }

                if (x.Count == row.hasSizes.Count + 1)
                {
                    return new List<List<int>>
                    {
                        x
                    };
                }

                if (x.Count == row.hasSizes.Count - 1)
                {
                    return new List<List<int>>
                    {
                        x.Append(0).Prepend(0).ToList()
                    };
                }

                return new List<List<int>>();
            }).ToList();

            var tests = new List<string>();
            foreach (var spaces in foo)
            {
                var sb = new StringBuilder();
                sb.Append(string.Join("",Enumerable.Repeat('.', spaces[0])));
                for (int i = 0; i < row.hasSizes.Count; i++)
                {
                    sb.Append(string.Join("", Enumerable.Repeat('#', row.hasSizes[i])));
                    sb.Append(string.Join("", Enumerable.Repeat('.', spaces[i+1])));
                }
                sb.Append(string.Join("",Enumerable.Repeat('.', spaces.Last())));
                
                tests.Add(sb.ToString());
            }

            var arrangements = 0;
            var inputArray = row.row.ToCharArray();
            foreach (var test in tests)
            {
                var testArray = test.ToCharArray();

                var isMatch = true;
                for (int i = 0; i < inputArray.Length; i++)
                {
                    if (inputArray[i] == '?')
                    {
                        continue;
                    }

                    if (inputArray[i] == testArray[i])
                    {
                        continue;
                    }

                    isMatch = false;
                }

                if (isMatch)
                {
                    arrangements++;
                }

            }

            totalArrangements += arrangements;
        }

        return totalArrangements.ToString();
    }

    public IEnumerable<List<int>> Combinations(int bag, int maximumCombos)
    {
        return Combinations(new List<int>(), bag, maximumCombos);
    }
    public IEnumerable<List<int>> Combinations(List<int> combos, int bag, int maximumCombos)
    {
        var minCombos = maximumCombos - 3;

        if (bag < minCombos - combos.Count)
        {
            yield break;
        }
        
        if (combos.Count > maximumCombos)
        {
            yield break;
        }
            
        

        switch (bag)
        {
            case 0:
                yield return combos;
                break;
            default:
            {
                foreach (var taken in Enumerable.Range(1, bag))
                {
                    var newCombos = combos.Select(x => x).ToList();
                    newCombos.Add(taken);
                    var newNewCombos = Combinations(newCombos, bag - taken, maximumCombos);
                    foreach (var newCombo in newNewCombos)
                    {
                        yield return newCombo;
                    }
                }

                break;
            }
        }
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var rows = (from row in input 
            select row.Split(' ') into split 
            let numbers = split[1].Split(',').Select(x => Convert.ToInt32(x)).ToList() 
            select new InputRow($"{split[0]}?{split[0]}?{split[0]}?{split[0]}?{split[0]}", Enumerable.Repeat(numbers,5).SelectMany(x => x).ToList())).ToList();

        var totalArrangements = 0;
        foreach (var row in rows)
        {
            var taken = row.hasSizes.Sum();
            var combos = Combinations(row.row.Length - taken, row.hasSizes.Count + 1);

            var foo = combos.Where(x => x.Count > row.hasSizes.Count - 2).SelectMany(x =>
            {
                if (x.Count == row.hasSizes.Count)
                {
                    return new List<List<int>>
                    {
                        x.Append(0).ToList(),
                        x.Prepend(0).ToList()
                    };
                }

                if (x.Count == row.hasSizes.Count + 1)
                {
                    return new List<List<int>>
                    {
                        x
                    };
                }

                if (x.Count == row.hasSizes.Count - 1)
                {
                    return new List<List<int>>
                    {
                        x.Append(0).Prepend(0).ToList()
                    };
                }

                return new List<List<int>>();
            }).ToList();

            var tests = new List<string>();
            foreach (var spaces in foo)
            {
                var sb = new StringBuilder();
                sb.Append(string.Join("",Enumerable.Repeat('.', spaces[0])));
                for (int i = 0; i < row.hasSizes.Count; i++)
                {
                    sb.Append(string.Join("", Enumerable.Repeat('#', row.hasSizes[i])));
                    sb.Append(string.Join("", Enumerable.Repeat('.', spaces[i+1])));
                }
                sb.Append(string.Join("",Enumerable.Repeat('.', spaces.Last())));
                
                tests.Add(sb.ToString());
            }

            var arrangements = 0;
            var inputArray = row.row.ToCharArray();
            for (var j = 0; j < tests.Count; j++)
            {
                var testArray = tests[j].ToCharArray();

                var isMatch = true;
                for (int i = 0; i < inputArray.Length; i++)
                {
                    if (inputArray[i] == '?')
                    {
                        continue;
                    }

                    if (inputArray[i] == testArray[i])
                    {
                        continue;
                    }

                    isMatch = false;
                    // var failed = string.Join("",inputArray.Take(i));
                    // tests = tests.Where((x, idx) => idx > j && !x.StartsWith(failed)).ToList();
                    break;
                }

                if (isMatch)
                {
                    arrangements++;
                }


            }

            totalArrangements += arrangements;
        }

        return totalArrangements.ToString();
    }

    public int Day => 12;
}
