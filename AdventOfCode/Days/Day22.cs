
using Location3D = (int x, int y, int z);

namespace AdventOfCode.Days;

public class Day22 : ISolution
{
    public record Brick(Location3D start, Location3D end);
    public string PartOne(IEnumerable<string> input)
    {
        var bricks = from row in input
            select row.Split('~')
            into split
            let startSplit = split[0].Split(',')
            let endSplit = split[1].Split(',')
            let start = (int.Parse(startSplit[0]), int.Parse(startSplit[1]), int.Parse(startSplit[2]))
            let end = (int.Parse(endSplit[0]), int.Parse(endSplit[1]), int.Parse(endSplit[2]))
            select new Brick(start, end);
        
        //Assumption all bricks have lowest z in start
        //nb all bricks that are stoop up are 1 by 1
        //No point starting at one as they have already settled.

        var (settled,_) = SettleBricks(bricks);

        settled = settled.OrderBy(brick => brick.start.z).ToList();

        var bricksThatCanBeRemoved = settled
            .Select(brick => settled.Where(b => b != brick))
            .Count(testSet => !HasUnSettledBricks(testSet));


        return bricksThatCanBeRemoved.ToString();
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var bricks = from row in input
            select row.Split('~')
            into split
            let startSplit = split[0].Split(',')
            let endSplit = split[1].Split(',')
            let start = (int.Parse(startSplit[0]), int.Parse(startSplit[1]), int.Parse(startSplit[2]))
            let end = (int.Parse(endSplit[0]), int.Parse(endSplit[1]), int.Parse(endSplit[2]))
            select new Brick(start, end);
        
        //Assumption all bricks have lowest z in start
        //nb all bricks that are stoop up are 1 by 1
        //No point starting at one as they have already settled.

        var (settled,_) = SettleBricks(bricks);

        settled = settled.OrderBy(brick => brick.start.z).ToList();

        var whenTheBricksFell = 0;
        foreach (var testSet in settled.Select(brick => settled.Where(b => b != brick)))
        {
            var (_, movementCount) = SettleBricks(testSet);
            whenTheBricksFell += movementCount;
        }
        
        return whenTheBricksFell.ToString();
    }


    private bool HasUnSettledBricks(IEnumerable<Brick> bricks)
    {
        var unsettled = new Queue<Brick>(bricks.OrderBy(x => x.start.z).ToList());
        var settled = new List<Brick>();

        while (unsettled.Count > 0)
        {
            var brick = unsettled.Dequeue();
            
            var hasSettled = false;
            while (!hasSettled)
            {
                //base case, already settled
                if (brick.start.z == 1 || brick.end.z == 1)
                {
                    settled.Add(brick);
                    hasSettled = true;
                }
                else
                {
                    var currentZ = brick.start.z;

                    var belowZ = currentZ - 1;
                    var projection2D = Project2d(brick).ToList();

                    var bricksBelow = settled.Where(b => belowZ == b.end.z).ToList();
                    var settledProjection = bricksBelow.SelectMany(Project2d).ToList();

                    if (settledProjection.Intersect(projection2D).Any())
                    {
                        hasSettled = true;
                        settled.Add(brick);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    private (List<Brick> bricks, int movementCount) SettleBricks(IEnumerable<Brick> bricks)
    {
        var unsettled = new Queue<Brick>(bricks.OrderBy(x => x.start.z).ToList());
        var settled = new List<Brick>();
        var movementCount = 0;

        while (unsettled.Count > 0)
        {
            var brick = unsettled.Dequeue();

            var moved = false;
            var hasSettled = false;
            while (!hasSettled)
            {
                //base case, already settled
                if (brick.start.z == 1 || brick.end.z == 1)
                {
                    settled.Add(brick);
                    hasSettled = true;
                }
                else
                {
                    var currentZ = brick.start.z;

                    var belowZ = currentZ - 1;
                    var projection2D = Project2d(brick).ToList();

                    var bricksBelow = settled.Where(b => belowZ == b.end.z).ToList();
                    var settledProjection = bricksBelow.SelectMany(Project2d).ToList();

                    if (settledProjection.Intersect(projection2D).Any())
                    {
                        hasSettled = true;
                        settled.Add(brick);
                    }
                    else
                    {
                        moved = true;
                        brick = new Brick((brick.start.x, brick.start.y, brick.start.z - 1),
                            (brick.end.x, brick.end.y, brick.end.z - 1));
                    }
                }
            }

            if (moved)
            {
                movementCount++;
            }
        }

        return (settled, movementCount);
    }
    
    private IEnumerable<Location> Project2d(Brick brick)
    {
        for (int x = brick.start.x; x <= brick.end.x; x++)
        for (int y = brick.start.y; y <= brick.end.y; y++)
        {
            yield return (x, y);
        }
    }

    public int Day => 22;
}
