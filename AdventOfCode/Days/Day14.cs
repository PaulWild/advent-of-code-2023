using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day14 : ISolution
{
    public string PartOne(IEnumerable<string> input)
    {
        var maxY = input.Count();
        var maxX = input.First().Length;
        Queue<(int x, int y)> locations = new();
        HashSet<(int x, int y)> blocks = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square == 'O')
            {
                locations.Enqueue((x,y));
            }
            if (square == '#')
            {
                blocks.Add((x,y));
            }
        }


        
        List<(int x, int y)> moved = new();
        do
        {
                var location = locations.Dequeue();
            
                if (location.y == 0)
                {
                    moved.Add(location);
                    // at the end
                } else if (blocks.Contains((location.x, location.y-1)))
                {
                    moved.Add(location);
                    //block in the way
                }
                else if (moved.Contains((location.x , location.y-1)))
                {
                    moved.Add(location);
                    // another doo dah in the way
                }
                else
                {
                    locations.Enqueue((location.x , location.y-1));
                }
                
        } while (locations.Count > 0);
        

        return moved.Select((loc) => maxY - loc.y).Sum().ToString();

        
        
    }

    public string PartTwo(IEnumerable<string> input)
    {
        var maxY = input.Count();
        var maxX = input.First().Length;
        Queue<(int x, int y)> locations = new();
        HashSet<(int x, int y)> blocks = new();
        foreach (var (row, y) in input.WithIndex())
        foreach (var (square, x) in row.ToCharArray().WithIndex())
        {
            if (square == 'O')
            {
                locations.Enqueue((x,y));
            }
            if (square == '#')
            {
                blocks.Add((x,y));
            }
        }

        List<int> weights = new();
        List<(int x, int y)> moved = new();
        //Assume the pattern is repeating after 1000 interations
        for (int i = 0; i < 300; i++)
        {
            //North
            do
            {
                var location = locations.Dequeue();

                if (location.y == 0)
                {
                    moved.Add(location);
                    // at the end
                }
                else if (blocks.Contains((location.x, location.y - 1)))
                {
                    moved.Add(location);
                    //block in the way
                }
                else if (moved.Contains((location.x, location.y - 1)))
                {
                    moved.Add(location);
                    // another doo dah in the way
                }
                else
                {
                    locations.Enqueue((location.x, location.y - 1));
                }

            } while (locations.Count > 0);
            
            
            locations = new Queue<(int x, int y)>(moved.OrderBy(loc => loc.x));
            moved = new();
            
            //West
            do
            {
                var location = locations.Dequeue();

                if (location.x == 0)
                {
                    moved.Add(location);
                    // at the end
                }
                else if (blocks.Contains((location.x-1, location.y)))
                {
                    moved.Add(location);
                    //block in the way
                }
                else if (moved.Contains((location.x-1, location.y )))
                {
                    moved.Add(location);
                    // another doo dah in the way
                }
                else
                {
                    locations.Enqueue((location.x-1, location.y ));
                }

            } while (locations.Count > 0);
           
            locations = new Queue<(int x, int y)>(moved.OrderByDescending(loc => loc.y));
            moved = new();
            //South
            do
            {
                var location = locations.Dequeue();

                if (location.y == maxY-1)
                {
                    moved.Add(location);
                    // at the end
                }
                else if (blocks.Contains((location.x, location.y + 1)))
                {
                    moved.Add(location);
                    //block in the way
                }
                else if (moved.Contains((location.x, location.y + 1)))
                {
                    moved.Add(location);
                    // another doo dah in the way
                }
                else
                {
                    locations.Enqueue((location.x, location.y + 1));
                }

            } while (locations.Count > 0);
            
            locations = new Queue<(int x, int y)>(moved.OrderByDescending(loc => loc.x));
            moved = new();
            
            //East
            do
            {
                var location = locations.Dequeue();

                if (location.x == maxX-1)
                {
                    moved.Add(location);
                    // at the end
                }
                else if (blocks.Contains((location.x+1, location.y)))
                {
                    moved.Add(location);
                    //block in the way
                }
                else if (moved.Contains((location.x+1, location.y)))
                {
                    moved.Add(location);
                    // another doo dah in the way
                }
                else
                {
                    locations.Enqueue((location.x+1, location.y));
                }

            } while (locations.Count > 0);
            
 
            
            locations = new Queue<(int x, int y)>(moved.OrderBy(loc => loc.y));
            moved = new();
            
            weights.Add(locations.Select((loc) => maxY - loc.y).Sum());

        
        }

        //Work out the loop size
        weights.Reverse();
        var loopNumber = 0;
        for (int i = 2; i < 150; i++)
        {
            var isLoop = true;
            for (int j = 0; j < i; j++)
            {
                if (weights[j] == weights[i + j])
                {
                    
                }
                else
                {
                    isLoop = false;
                }
            }

            if (isLoop)
            {
                loopNumber = i;
                break;
            }
        }
        

        //work out where the loop starts
        weights.Reverse();

        var startPoint = 0;
        for (int i = 0; i < 150; i++)
        {
            var start = true;
            for (int j = 0; j < loopNumber; j++)
            {
                if (weights[i + j] != weights[i + j + loopNumber])
                {
                    start = false;
                }
            }

            if (start == true)
            {
                startPoint = i;
                break;
            }
        }

        var loop = weights.Skip(startPoint).Take(loopNumber).ToList();
        var remainder = (1000000000 - startPoint-1) % loopNumber;
        
        
        
      
        return loop[remainder].ToString();  
        
       
    }

    public int Day => 14;
}
