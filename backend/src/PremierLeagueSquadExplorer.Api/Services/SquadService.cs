using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Options;

namespace PremierLeagueSquadExplorer.Api.Services;

public sealed class SquadService(
    ITeamResolverService teamResolverService,
    IFootballApiClient footballApiClient,
    IOptions<FootballApiOptions> footballApiOptions,
    IMemoryCache cache) : ISquadService
{
    private const string UnknownValue = "Unknown";

    private static readonly TimeSpan SquadCacheDuration = TimeSpan.FromMinutes(30);

    private readonly IMemoryCache _cache = cache;

    private readonly ITeamResolverService _teamResolverService = teamResolverService;
    private readonly IFootballApiClient _footballApiClient = footballApiClient;
    private readonly FootballApiOptions _footballApiOptions = footballApiOptions.Value;

    public async Task<SquadDto?> GetSquadAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var team = await _teamResolverService.ResolveAsync(query, cancellationToken);

        if (team is null)
        {
            return null;
        }

        var cacheKey = CacheKeys.SquadByTeam(
            team.Id,
            _footballApiOptions.LeagueId,
            _footballApiOptions.Season);

        if (_cache.TryGetValue<SquadDto>(cacheKey, out var cachedSquad) &&
            cachedSquad is not null)
        {
            return WithCachedMetadata(cachedSquad);
        }

        var players = await _footballApiClient.GetPlayersByTeamAsync(
            team.Id,
            cancellationToken);

        // The /players endpoint can return multiple season records for the same player.
        // The UI should show one squad card per player, so we de-duplicate by player ID.
        var playerDtos = players
            .Where(item => item.Player.Id > 0)
            .GroupBy(item => item.Player.Id)
            .Select(group => MapPlayer(group.First()))
            .OrderBy(player => player.Position)
            .ThenBy(player => player.Surname)
            .ThenBy(player => player.FirstName)
            .ToArray();

        var squad = new SquadDto
        {
            Team = team,
            Players = playerDtos,
            Metadata = CreateMetadata(cached: false)
        };

        _cache.Set(cacheKey, squad, SquadCacheDuration);

        return squad;
    }

    private SquadMetadataDto CreateMetadata(bool cached)
    {
        return new SquadMetadataDto
        {
            LeagueId = _footballApiOptions.LeagueId,
            Season = _footballApiOptions.Season,
            Source = FootballProviderNames.ApiFootball,
            Cached = cached
        };
    }

    private SquadDto WithCachedMetadata(SquadDto squad)
    {
        return new SquadDto
        {
            Team = squad.Team,
            Players = squad.Players,
            Metadata = CreateMetadata(cached: true)
        };
    }

    private static PlayerDto MapPlayer(ApiFootballPlayerItem item)
    {
        var player = item.Player;
        var position = item.Statistics
            .FirstOrDefault()
            ?.Games
            .Position;

        return new PlayerDto
        {
            Id = player.Id,
            FirstName = player.Firstname?.Trim() ?? string.Empty,
            Surname = player.Lastname?.Trim() ?? string.Empty,
            DateOfBirth = string.IsNullOrWhiteSpace(player.Birth?.Date)
                ? null
                : player.Birth.Date,
            Position = string.IsNullOrWhiteSpace(position)
                ? UnknownValue
                : position,
            ProfilePictureUrl = string.IsNullOrWhiteSpace(player.Photo)
                ? null
                : player.Photo,
            DisplayName = BuildDisplayName(player)
        };
    }

    private static string BuildDisplayName(ApiFootballPlayer player)
    {
        if (!string.IsNullOrWhiteSpace(player.Name))
            return player.Name;

        var fullName = $"{player.Firstname} {player.Lastname}".Trim();

        return string.IsNullOrWhiteSpace(fullName)
            ? UnknownValue
            : fullName;
    }
}