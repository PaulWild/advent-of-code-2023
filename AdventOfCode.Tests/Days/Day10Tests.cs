using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day10Tests
{
    private readonly ISolution _sut = new Day10();
    
    private readonly string[] _testData =
    {
        "..F7.", 
        ".FJ|.",
        "SJ.L7",
        "|F--J",
        "LJ...",
    };

    private readonly string[] _testDataPartTwo =
    {
        "...........",
        ".S-------7.",
        ".|F-----7|.",
        ".||.....||.",
        ".||.....||.",
        ".|L-7.F-J|.",
        ".|..|.|..|.",
        ".L--J.L--J.",
        "..........."
    };

    private readonly string[] _testDataPartTwoExampleTwo =
    {
        "FF7FSF7F7F7F7F7F---7",
        "L|LJ||||||||||||F--J",
        "FL-7LJLJ||||||LJL-77",
        "F--JF--7||LJLJ7F7FJ-",
        "L---JF-JLJ.||-FJLJJ7",
        "|F|F-JF---7F7-L7L|7|",
        "|FFJF7L7F-JF7|JL---7",
        "7-L-JL7||F7|L7F-7F7|",
        "L.L7LFJ|||||FJL7||LJ",
        "L7JLJL-JLJLJL--JLJ.L"
    };

    private readonly string[] _testDataPartTwoExampleThree =
    {
        ".F----7F7F7F7F-7....",
        ".|F--7||||||||FJ....",
        ".||.FJ||||||||L7....",
        "FJL7L7LJLJ||LJ.L-7..",
        "L--J.L7...LJS7F-7L7.",
        "....F-J..F7FJ|L7L7L7",
        "....L7.F7||L7|.L7L7|",
        ".....|FJLJ|FJ|F7|.LJ",
        "....FJL-7.||.||||...",
        "....L---J.LJ.LJLJ..."
    };

    [Fact]
    public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartOne(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartOne_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartOne(_testData);

        actual.Should().Be("8");
    }


    [Fact]
    public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartTwo(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartTwo(_testDataPartTwo);

        actual.Should().Be("4");
    }
    
    [Fact]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswerTwo()
    {
        var actual = _sut.PartTwo(_testDataPartTwoExampleTwo);

        actual.Should().Be("10");
    }
    
    [Fact]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswerThree()
    {
        var actual = _sut.PartTwo(_testDataPartTwoExampleThree);

        actual.Should().Be("8");
    }
}