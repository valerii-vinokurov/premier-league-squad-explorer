using Microsoft.Extensions.Caching.Memory;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;
using PremierLeagueSquadExplorer.Api.Services;
using PremierLeagueSquadExplorer.Api.Tests.Fakers;
using PremierLeagueSquadExplorer.Api.Tests.TestData;

namespace PremierLeagueSquadExplorer.Api.Tests.Services;

public sealed class TeamResolverServiceTests
{
    [Theory]
    [InlineData("West Ham United")]
    [InlineData("West Ham")]
    [InlineData("The Hammers")]
    [InlineData("Hammers")]
    [InlineData("hammers")]
    [InlineData("  THE   HAMMERS  ")]
    public async Task ResolveAsync_ShouldResolveWestHamOfficialNameProviderNameAndNicknames(
        string query)
    {
        var service = CreateService();

        var result = await service.ResolveAsync(query);

        Assert.NotNull(result);
        Assert.Equal(48, result.Id);
        Assert.Equal("West Ham United", result.Name);
        Assert.Equal("West Ham", result.ProviderName);
        Assert.Equal("https://example.test/west-ham.png", result.LogoUrl);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Unknown Team")]
    public async Task ResolveAsync_ShouldReturnNull_WhenQueryIsInvalidOrUnknown(
        string query)
    {
        var service = CreateService();

        var result = await service.ResolveAsync(query);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTeamsAsync_ShouldReturnResolvedTeams()
    {
        var service = CreateService();

        var result = await service.GetTeamsAsync();

        var team = Assert.Single(result);

        Assert.Equal(48, team.Id);
        Assert.Equal("West Ham United", team.Name);
        Assert.Equal("West Ham", team.ProviderName);
    }

    [Fact]
    public async Task GetTeamsAsync_ShouldReturnEmptyCollection_WhenProviderTeamIsMissing()
    {
        var service = CreateService(providerTeams: []);

        var result = await service.GetTeamsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static TeamResolverService CreateService(
        IReadOnlyCollection<ApiFootballTeamItem>? providerTeams = null)
    {
        var footballApiClient = new FakeFootballApiClient(
            teams: providerTeams ?? ApiFootballTestData.CreateWestHamProviderTeams());

        var aliasProvider = new FakeTeamAliasProvider(
            TeamAliasTestData.CreateWestHamAliases());

        var cache = new MemoryCache(new MemoryCacheOptions());

        return new TeamResolverService(
            footballApiClient,
            aliasProvider,
            cache,
            TestFootballApiOptions.Create());
    }
}