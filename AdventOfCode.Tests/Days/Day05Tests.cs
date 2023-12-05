using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day05Tests
{
    private readonly ISolution _sut = new Day05();
    
    private readonly string[] _testData =
    {
        "seeds: 79 14 55 13",
"",
        "seed-to-soil map:",
        "50 98 2",
        "52 50 48",
"",
        "soil-to-fertilizer map:",
        "0 15 37",
        "37 52 2",
        "39 0 15",
"",
        "fertilizer-to-water map:",
        "49 53 8",
        "0 11 42",
        "42 0 7",
        "57 7 4",
"",
        "water-to-light map:",
        "88 18 7",
        "18 25 70",
"",
        "light-to-temperature map:",
        "45 77 23",
        "81 45 19",
        "68 64 13",
"",
        "temperature-to-humidity map:",
        "0 69 1",
        "1 0 69",
"",
        "humidity-to-location map:",
        "60 56 37",
        "56 93 4"
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

        actual.Should().Be("35");
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
        var actual = _sut.PartTwo(_testData);

        actual.Should().Be("46");
    }
}