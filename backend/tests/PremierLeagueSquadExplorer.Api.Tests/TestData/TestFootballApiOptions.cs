using Microsoft.Extensions.Options;
using PremierLeagueSquadExplorer.Api.Options;

namespace PremierLeagueSquadExplorer.Api.Tests.TestData;

internal static class TestFootballApiOptions
{
    internal static IOptions<FootballApiOptions> Create()
        => Microsoft.Extensions.Options.Options.Create(new FootballApiOptions
        {
            BaseUrl = "https://example.test",
            ApiKey = "test-key",
            LeagueId = 39,
            Season = 2024,
            MaxPlayerPages = 3
        });
}