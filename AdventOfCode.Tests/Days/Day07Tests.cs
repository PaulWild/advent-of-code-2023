using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day07Tests
{
    private readonly ISolution _sut = new Day07();
    
    private readonly string[] _testData =
    {
        "32T3K 765",
        "T55J5 684",
        "KK677 28",
        "KTJJT 220",
        "QQQJA 483"
    };


    [Fact]
    public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartOne(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Theory]
    [InlineData(new []{"T", "6"}, new []{"6", "T"})]
    [InlineData(new []{"5", "4"}, new []{"4", "5"})]
    [InlineData(new []{"Q", "K"}, new []{"Q", "K"})]
    [InlineData(new []{"6", "T"}, new []{"6", "T"})]
    [InlineData(new []{"A", "K"}, new []{"K", "A"})]  
    [InlineData(new []{"QQQJA", "T55J5"}, new []{"T55J5", "QQQJA"})]
    public void Ordering(string[] input, string[] expected)
    {
        
        var ordered = input.OrderBy(x => x, new Day07.CardComparer(Day07.Part1Comparer));
        ordered.Should().Equal(expected);
        
    }
    
    [Fact]
    public void PartOne_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartOne(_testData);

        actual.Should().Be("6440");
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

        actual.Should().Be("5905");
    }
}