using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day07 : ISolution
{
    public static readonly Dictionary<char, int> Part1Comparer = new()
    {
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        { 'J', 11 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 }
    };
    
    private static readonly Dictionary<char, int> Part2Comparer = new()
    {
        { 'J', 1 },
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 }
    };
    
    public class CardComparer(IReadOnlyDictionary<char, int> toNumbers) : IComparer<string>
    {
        public int Compare(string? left, string? right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            
            var xChars = left.ToCharArray();
            var yChars = right.ToCharArray();
            
            for (int i = 0; i < xChars.Length; i++)
            {
                var x = toNumbers[xChars[i]];
                var y = toNumbers[yChars[i]];

                if (x == y)
                {
                    continue;
                }

                return x > y ? 1 : -1;
            }

            return 0;
        }
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        List<(string hand, int bid)> hands = input
            .Select(row => row.Split(" "))
            .Select(split => (split[0], Convert.ToInt32(split[1])))
            .ToList();

        List<(string hand, int bid, double result)> handsWithScores = new();
        
        foreach (var (hand, bid) in hands)
        {
            var cards = hand.ToCharArray();
            var grouped = cards.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            var handResult = 0.0;
            if (grouped.Values.Count(x => x == 2) == 2)
            {
                //assign slightly better weight to two pair
                handResult = 2.5;
            }
            else if (grouped.Values.Contains(3) && grouped.Values.Contains(2))
            {
                //assign slightly better weight to full house
                handResult = 3.5;
            }
            else
            {
                handResult = grouped.Values.Max();
            }
            handsWithScores.Add((hand, bid, handResult));
        }

        var sorted = handsWithScores
            .OrderBy(x => x.result)
            .ThenBy(x => x.hand, new CardComparer(Part1Comparer));

        var toReturn = 0;
        foreach (var (handWithResult, idx) in sorted.WithIndex())
        {
            toReturn += (idx + 1) * handWithResult.bid;
        }

        
        return toReturn.ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        List<(string hand, int bid)> hands = input
            .Select(row => row.Split(" "))
            .Select(split => (split[0], Convert.ToInt32(split[1])))
            .ToList();

        List<(string hand, int bid, double result)> handsWithScores = new();
        
        foreach (var (hand, bid) in hands)
        {
            var cards = hand.ToCharArray();
            var grouped = cards.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            var jacks = cards.Count(x => x == 'J');

            if (jacks != 5)
            {
                var max = grouped.ToList()
                    .Where(x => x.Key != 'J')
                    .Select(x => x.Value)
                    .Max();

                var highest = grouped.First(x => x.Value == max && x.Key != 'J');
                grouped[highest.Key] += jacks;

                grouped.Remove('J');
            }
            
            var handResult = 0.0;
            if (grouped.Values.Count(x => x == 2) == 2)
            {
                //assign slightly better weight to two pair
                handResult = 2.5;
            }
            else if (grouped.Values.Contains(3) && grouped.Values.Contains(2))
            {
                //assign slightly better weight to full house
                handResult = 3.5;
            }
            else
            {
                handResult = grouped.Values.Max();
            }
            handsWithScores.Add((hand, bid, handResult));
        }

        var sorted = handsWithScores
            .OrderBy(x => x.result)
            .ThenBy(x => x.hand, new CardComparer(Part2Comparer));

        var toReturn = 0;
        foreach (var (handWithResult, idx) in sorted.WithIndex())
        {
            toReturn += (idx + 1) * handWithResult.bid;
        }

        
        return toReturn.ToString();
    }

    public int Day => 07;
}
