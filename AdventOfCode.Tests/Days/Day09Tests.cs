using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day09Tests
{
    private readonly ISolution _sut = new Day09();
    
    private readonly string[] _testData =
    {
        "0 3 6 9 12 15",
        "1 3 6 10 15 21",
        "10 13 16 21 30 45"
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

        actual.Should().Be("114");
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

        actual.Should().Be("2");
    }
}