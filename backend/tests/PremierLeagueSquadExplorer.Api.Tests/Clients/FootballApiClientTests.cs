using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Options;
using PremierLeagueSquadExplorer.Api.Tests.Fakers;

namespace PremierLeagueSquadExplorer.Api.Tests.Clients;

public sealed class FootballApiClientTests
{
    [Fact]
    public async Task GetPlayersByTeamAsync_ShouldNotRequestPagesBeyondConfiguredLimit()
    {
        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(
            """
            {
              "get": "players",
              "parameters": {
                "team": "48",
                "league": "39",
                "season": "2024",
                "page": 1
              },
              "errors": [],
              "results": 0,
              "paging": {
                "current": 1,
                "total": 5
              },
              "response": []
            }
            """));

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test/")
        };

        var options = Microsoft.Extensions.Options.Options.Create(new FootballApiOptions
        {
            BaseUrl = "https://example.test",
            ApiKey = "test-key",
            LeagueId = 39,
            Season = 2024,
            MaxPlayerPages = 3
        });

        var client = new FootballApiClient(httpClient, options);

        await client.GetPlayersByTeamAsync(48);

        Assert.Equal(3, handler.RequestedUris.Count);

        Assert.Contains(handler.RequestedUris, uri => uri.Query.Contains("page=1"));
        Assert.Contains(handler.RequestedUris, uri => uri.Query.Contains("page=2"));
        Assert.Contains(handler.RequestedUris, uri => uri.Query.Contains("page=3"));

        Assert.DoesNotContain(handler.RequestedUris, uri => uri.Query.Contains("page=4"));
        Assert.DoesNotContain(handler.RequestedUris, uri => uri.Query.Contains("page=5"));
    }
}