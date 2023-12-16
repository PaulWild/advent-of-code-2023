using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day12Tests
{
    private readonly ISolution _sut = new Day12();
    
    private readonly string[] _testData =
    {
        "?#????????????##?? 1,1,10",

     
    };


    [Fact]
    public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartOne(_sut.Input());

        act.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("???.### 1,1,3", "1")]
    [InlineData(".??..??...?##. 1,1,3", "4")]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", "1")]
    [InlineData("????.#...#... 4,1,1", "1")]
    [InlineData("????.######..#####. 1,6,5", "4")]
    [InlineData("?###???????? 3,2,1", "10")]
    [InlineData("#.?# 1,1", "1")]
    public void PartOne_WhenCalled_ReturnsCorrectTestAnswer(string input, string expected)
    {
        var actual = _sut.PartOne([input]);

        actual.Should().Be(expected);
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

        actual.Should().Be("11880925");
    }

}