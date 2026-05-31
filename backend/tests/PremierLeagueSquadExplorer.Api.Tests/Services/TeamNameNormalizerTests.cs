using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Tests.Services;

public sealed class TeamNameNormalizerTests
{
    [Theory]
    [InlineData("The Hammers", "the hammers")]
    [InlineData("  THE   HAMMERS  ", "the hammers")]
    [InlineData("Brighton & Hove Albion", "brighton and hove albion")]
    [InlineData("West-Ham United", "west ham united")]
    [InlineData("West.Ham, United!", "west ham united")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void Normalize_ShouldReturnExpectedValue(string input, string expected)
    {
        var result = TeamNameNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Normalize_ShouldReturnEmptyString_WhenValueIsNull()
    {
        var result = TeamNameNormalizer.Normalize(null);

        Assert.Equal(string.Empty, result);
    }
}