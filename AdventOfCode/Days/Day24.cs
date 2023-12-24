using Point = (decimal x, decimal y, decimal z);
using Diff = (int dx, int dy, int dz);

using System.Text.RegularExpressions;

namespace AdventOfCode.Days;

public class Day24 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {

        var hailStorms = new List<(Point point, Diff diff, decimal m, decimal c)>();
        foreach (var row in input)
        {
            var matches = Regex.Matches(row, "-?[0-9]+");
            Point point = (decimal.Parse(matches[0].Value), decimal.Parse(matches[1].Value),
                decimal.Parse(matches[2].Value));
            
            Diff diff = (int.Parse(matches[3].Value), int.Parse(matches[4].Value),
                int.Parse(matches[5].Value));
       
            var m = diff.dy / (decimal)diff.dx;
            var c = point.y - (m * point.x);
            hailStorms.Add((point, diff, m, c));
        }

        var combinations = new List<((Point point, Diff diff, decimal m, decimal c), (Point point, Diff diff, decimal m, decimal c))>();
        for (var i = 0; i < hailStorms.Count; i++)
        for (var j = i+1; j < hailStorms.Count; j++)
        {
            combinations.Add((hailStorms[i], hailStorms[j]));
        }

        var validIntersections = 0;
        foreach (var (hailstorm1, hailstorm2) in combinations)
        {
            // ax +c = bx + d 
            // x = (d-c)/(a-b)
            // y = a * (d-c)/(a-b) + c

            var a = hailstorm1.m;
            var b = hailstorm2.m;
            var c = hailstorm1.c;
            var d = hailstorm2.c;

            //lines are parralel
            if (a == b)
            {
                continue;
            }

            var xPoint = (d - c) / (a - b);
            var yPoint = a * ((d - c) / (a - b)) + c;

            var min = 200_000_000_000_000;
            var max = 400_000_000_000_000;

            // In the past
            if ((xPoint < hailstorm1.point.x && hailstorm1.diff.dx >= 0) 
                || (xPoint > hailstorm1.point.x && hailstorm1.diff.dx < 0) 
                || (xPoint < hailstorm2.point.x && hailstorm2.diff.dx >= 0) 
                || (xPoint > hailstorm2.point.x && hailstorm2.diff.dx < 0))
            {
                continue;
            }

            if (xPoint >= min && xPoint <= max && yPoint >= min && yPoint <= max)
            {
                validIntersections++;
            }
        }


        return (validIntersections).ToString();
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var hailStorms = new List<(Point point, Diff diff, decimal m, decimal c)>();
        foreach (var row in input)
        {
            var matches = Regex.Matches(row, "-?[0-9]+");
            Point point = (decimal.Parse(matches[0].Value), decimal.Parse(matches[1].Value),
                decimal.Parse(matches[2].Value));
            
            Diff diff = (int.Parse(matches[3].Value), int.Parse(matches[4].Value),
                int.Parse(matches[5].Value));
       
            var m = diff.dy / (decimal)diff.dx;
            var c = point.y - (m * point.x);
            hailStorms.Add((point, diff, m, c));
        }
        
        var combinations = new List<((Point point, Diff diff, decimal m, decimal c), (Point point, Diff diff, decimal m, decimal c))>();
        for (var i = 0; i < hailStorms.Count; i++)
        for (var j = i+1; j < hailStorms.Count; j++)
        {
            combinations.Add((hailStorms[i], hailStorms[j]));
        }
        
        //Had to look up some hints, if two rays have the same velocity and start point, then the intersecting ray
        //has to as well. It's in the y coordinates for me. 
        
        var sameY = combinations.First(combo =>
            combo.Item1.point.y == combo.Item2.point.y && combo.Item1.diff.dy == combo.Item2.diff.dy);

        var y = sameY.Item1.point.y;
        var vy = sameY.Item1.diff.dy;

        //assuming the first two rays arent the one we found. yolo
        
        // First ray
        var firstRay = hailStorms[0];
        
        var interceptTime = (y - firstRay.point.y) / (firstRay.diff.dy - vy);
        var xIntercept1 = interceptTime * firstRay.diff.dx + firstRay.point.x;
        var zIntercept1 = interceptTime * firstRay.diff.dz + firstRay.point.z;
        
        // Second ray
        var secondRay = hailStorms[1];
        
        var secondInterceptTime = (y - secondRay.point.y) / (secondRay.diff.dy -vy);
        var xIntercept2 = secondInterceptTime * secondRay.diff.dx + secondRay.point.x;
        var zIntercept2 = secondInterceptTime * secondRay.diff.dz + secondRay.point.z;


        var vx = (xIntercept2 - xIntercept1)/(secondInterceptTime - interceptTime);
        var vz = (zIntercept2 - zIntercept1)/(secondInterceptTime - interceptTime);

        var x = xIntercept1 - vx * interceptTime;
        var z = zIntercept1 - vz * interceptTime;
        
        return (x + y + z).ToString();
    }

    public int Day => 24;
}
