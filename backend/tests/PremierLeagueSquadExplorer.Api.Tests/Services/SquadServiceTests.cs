using Microsoft.Extensions.Caching.Memory;
using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Exceptions;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Services;
using PremierLeagueSquadExplorer.Api.Tests.Fakers;
using PremierLeagueSquadExplorer.Api.Tests.TestData;

namespace PremierLeagueSquadExplorer.Api.Tests.Services;

public sealed class SquadServiceTests
{
    [Fact]
    public async Task GetSquadAsync_ShouldReturnNull_WhenTeamCannotBeResolved()
    {
        var service = CreateService(resolvedTeam: null);

        var result = await service.GetSquadAsync("Unknown Team");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSquadAsync_ShouldMapMissingPlayerPhotoDobAndPositionDefensively()
    {
        var service = CreateService(
            resolvedTeam: CreateTeam(),
            players: CreatePlayersWithMissingData());

        var result = await service.GetSquadAsync("The Hammers");

        Assert.NotNull(result);

        var player = Assert.Single(result.Players);

        Assert.Equal(123, player.Id);
        Assert.Equal("Test", player.FirstName);
        Assert.Equal("Player", player.Surname);
        Assert.Null(player.ProfilePictureUrl);
        Assert.Null(player.DateOfBirth);
        Assert.Equal("Unknown", player.Position);
        Assert.Equal("T. Player", player.DisplayName);

        Assert.Equal(39, result.Metadata.LeagueId);
        Assert.Equal(2024, result.Metadata.Season);
        Assert.Equal(FootballProviderNames.ApiFootball, result.Metadata.Source);
        Assert.False(result.Metadata.Cached);
    }

    [Fact]
    public async Task GetSquadAsync_ShouldDeduplicatePlayersByProviderPlayerId()
    {
        var service = CreateService(
            resolvedTeam: CreateTeam(),
            players:
            [
                CreatePlayer(id: 123, firstName: "Test", lastName: "Player"),
                CreatePlayer(id: 123, firstName: "Test", lastName: "Player")
            ]);

        var result = await service.GetSquadAsync("The Hammers");

        Assert.NotNull(result);
        Assert.Single(result.Players);
    }

    [Fact]
    public async Task GetSquadAsync_ShouldReturnCachedSquadOnSecondRequest()
    {
        var footballApiClient = new FakeFootballApiClient(
            players: [CreatePlayer(id: 123, firstName: "Test", lastName: "Player")]);

        var service = CreateService(
            resolvedTeam: CreateTeam(),
            footballApiClient: footballApiClient);

        var firstResult = await service.GetSquadAsync("The Hammers");
        var secondResult = await service.GetSquadAsync("The Hammers");

        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult);

        Assert.False(firstResult.Metadata.Cached);
        Assert.True(secondResult.Metadata.Cached);
        Assert.Equal(1, footballApiClient.GetPlayersByTeamCallCount);
    }

    [Fact]
    public async Task GetSquadAsync_ShouldPropagateFootballApiException_WhenProviderFails()
    {
        var exception = new FootballApiException(
            "Football API returned provider errors.",
            providerErrors: """{"requests":"rate limit reached"}""");

        var service = CreateService(
            resolvedTeam: DtoTestData.CreateWestHamTeam(),
            footballApiClient: new FakeFootballApiClient(playersException: exception));

        await Assert.ThrowsAsync<FootballApiException>(
            () => service.GetSquadAsync("The Hammers"));
    }

    private static SquadService CreateService(
        TeamDto? resolvedTeam,
        IReadOnlyCollection<ApiFootballPlayerItem>? players = null,
        IFootballApiClient? footballApiClient = null)
    {
        var teamResolverService = new FakeTeamResolverService(resolvedTeam);

        var apiClient = footballApiClient ??
            new FakeFootballApiClient(players: players ?? []);

        var cache = new MemoryCache(new MemoryCacheOptions());

        return new SquadService(
            teamResolverService,
            apiClient,
            TestFootballApiOptions.Create(),
            cache);
    }

    private static TeamDto CreateTeam()
        => new()
        {
            Id = 48,
            Name = "West Ham United",
            ProviderName = "West Ham",
            LogoUrl = "https://example.test/west-ham.png"
        };

    private static IReadOnlyCollection<ApiFootballPlayerItem> CreatePlayersWithMissingData()
        =>
        [
            new ApiFootballPlayerItem
            {
                Player = new ApiFootballPlayer
                {
                    Id = 123,
                    Name = "T. Player",
                    Firstname = "Test",
                    Lastname = "Player",
                    Birth = new ApiFootballBirth
                    {
                        Date = null
                    },
                    Photo = null
                },
                Statistics =
                [
                    new ApiFootballStatistic
                    {
                        Games = new ApiFootballGames
                        {
                            Position = null
                        }
                    }
                ]
            }
        ];

    private static ApiFootballPlayerItem CreatePlayer(
        int id,
        string firstName,
        string lastName)
            => new()
            {
                Player = new ApiFootballPlayer
                {
                    Id = id,
                    Name = $"{firstName[0]}. {lastName}",
                    Firstname = firstName,
                    Lastname = lastName,
                    Birth = new ApiFootballBirth
                    {
                        Date = "2000-01-01"
                    },
                    Photo = "https://example.test/player.png"
                },
                Statistics =
                [
                    new ApiFootballStatistic
                    {
                        Games = new ApiFootballGames
                        {
                            Position = "Midfielder"
                        }
                    }
                ]
            };
}