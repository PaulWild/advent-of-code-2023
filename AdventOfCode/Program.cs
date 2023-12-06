using System.CommandLine;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using AdventOfCode;

const string year = "2023";
Console.OutputEncoding = Encoding.UTF8;

var dayArgument = new Argument<int?>("day", "Day to run.");
var partOption = new Option<int?>(new []{ "--part", "-p" }, "The part to run");
var aoc = new Command("aoc", "run advent of code solutions")
{
    dayArgument,
    partOption,
};
aoc.SetHandler(RunSolutionForDay, dayArgument, partOption);

var dayBootstrapArgument = new Argument<int?>("day", "day to bootstrap");
var bootstrap = new Command("bootstrap", "bootstrap a new advent of code day")
{
    dayBootstrapArgument
};

bootstrap.SetHandler(BootStrap, dayBootstrapArgument);

var root = new RootCommand
{
    aoc,
    bootstrap
};

            
return await root.InvokeAsync(args);
     

static void RunSolutionForDay(int? day, int? part)
{
    var solutions = GetSolutionsToRun(day);
    
    Console.WriteLine($"Advent Of Code");
    
    foreach (var solution in solutions)
    {
        Console.WriteLine($"Day {solution.Day}");

        if (part is null or 1)
        {
            RunSolutionPart(() => solution.PartOne(solution.Input()), 1);
        }
        if (part is null or 2)
        {
            RunSolutionPart(() => solution.PartTwo(solution.Input()), 2);
        }
    }
}

static IEnumerable<ISolution> GetSolutionsToRun(int? day)
{
    List<ISolution> solutions = new();
    if (day.HasValue)
    {
        var sol = Solutions().SingleOrDefault(x => x?.Day == day);
        if (sol != null)
        {
            solutions.Add(sol);
        }
    }
    else
    {
        solutions.AddRange(Solutions()!);
    }

    return solutions;
}

static void RunSolutionPart(Func<string> solutionFunc, int part)
{
    try
    {
        var timer = new Stopwatch();
        timer.Start();
        var answer = solutionFunc();
        timer.Stop();

        PrintSolution(part, answer, timer.ElapsedMilliseconds);
    }
    catch (NotImplementedException)
    {
        PrintError(part);
    }
}

static void PrintSolution(int part, string answer, long timeInMillis)
{
    Output.Write("  \u2714")
        .WithForegroundColour(ConsoleColor.Green)
        .Run();
    Output.Write($" - Part {part}: {answer} ")
        .Run();
    
    var icon = timeInMillis > 50
        ? "\u231B " 
        : "";
    
    var timeColor = timeInMillis > 50  
        ? timeInMillis > 1000 
            ? ConsoleColor.Red 
            : ConsoleColor.Yellow 
        : ConsoleColor.Green;
    
    Output.Write($"[{icon}{timeInMillis}ms] {Environment.NewLine}")
        .WithForegroundColour(timeColor)
        .Run();
}

static void PrintError(int part)
{
    Output.Write("  \u2718")
        .WithForegroundColour(ConsoleColor.Red)
        .Run();
    Output.Write($" - Part {part} has not been solved{Environment.NewLine}")
        .Run();
}

static IEnumerable<ISolution?> Solutions()
{
    var type = typeof(ISolution);

    return Assembly.GetExecutingAssembly().DefinedTypes
        .Where(x => x.ImplementedInterfaces.Contains(type))
        .Select(impl => (ISolution)Activator.CreateInstance(impl)!)
        .Where(x => x.Day != 0)
        .OrderBy(sol => sol?.Day);
}

static async Task BootStrap(int? day)
{
    day ??= DateTime.Now.Day; 
    
    var padding = day > 9 ? "" : "0";
    
    //Main File
    var text = await File.ReadAllTextAsync("./AdventOfCode/Days/Day00.cs");
    var newText = text.Replace("00", padding + day);
    await File.WriteAllTextAsync($"./AdventOfCode/Days/Day{padding}{day}.cs", newText);
   
    //Test File
    var testText = await File.ReadAllTextAsync("./AdventOfCode.Tests/Days/Day00Tests.cs");
    var newTestText = testText.Replace("00", padding + day).Replace("(Skip = \"Scaffold\")", "");
    await File.WriteAllTextAsync($"./AdventOfCode.Tests/Days/Day{padding}{day}Tests.cs", newTestText);
    
    //Input Data
    var session = Environment.GetEnvironmentVariable("AOC_SESSION_ID");
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("cookie", $"session={session}");
    
    var inputString = await client.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
    
    await File.WriteAllTextAsync($"./AdventOfCode/Input/day_{padding}{day}.txt", inputString);
}
    
