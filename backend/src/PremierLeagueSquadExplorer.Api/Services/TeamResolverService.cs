using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;
using PremierLeagueSquadExplorer.Api.Models.TeamLookup;
using PremierLeagueSquadExplorer.Api.Options;

namespace PremierLeagueSquadExplorer.Api.Services;

public sealed class TeamResolverService : ITeamResolverService
{
    private static readonly TimeSpan TeamsCacheDuration = TimeSpan.FromHours(12);

    private readonly IFootballApiClient _footballApiClient;
    private readonly IMemoryCache _cache;
    private readonly FootballApiOptions _footballApiOptions;
    private readonly IReadOnlyCollection<TeamAliasDefinition> _supportedTeams;
    private readonly IReadOnlyDictionary<string, TeamAliasDefinition> _teamLookup;

    public TeamResolverService(
        IFootballApiClient footballApiClient,
        ITeamAliasProvider teamAliasProvider,
        IMemoryCache cache,
        IOptions<FootballApiOptions> footballApiOptions)
    {
        _footballApiClient = footballApiClient;
        _cache = cache;
        _footballApiOptions = footballApiOptions.Value;
        _supportedTeams = teamAliasProvider.GetSupportedTeams();
        _teamLookup = BuildTeamLookup(_supportedTeams);
    }

    public async Task<IReadOnlyCollection<TeamDto>> GetTeamsAsync(CancellationToken cancellationToken = default)
    {
        var providerTeams = await GetProviderTeamsAsync(cancellationToken);
        var providerTeamLookup = BuildProviderTeamLookup(providerTeams);

        return [.. _supportedTeams
            .Select(team => MapToTeamDto(team, providerTeamLookup))
            .Where(team => team is not null)
            .Cast<TeamDto>()];
    }

    private async Task<IReadOnlyCollection<ApiFootballTeamItem>> GetProviderTeamsAsync(
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.PremierLeagueTeams(
            _footballApiOptions.LeagueId,
            _footballApiOptions.Season);

        var cachedTeams = await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TeamsCacheDuration;

            var teams = await _footballApiClient.GetPremierLeagueTeamsAsync(cancellationToken);

            return teams.ToArray();
        });

        return cachedTeams ?? [];
    }

    public async Task<TeamDto?> ResolveAsync(string query, CancellationToken cancellationToken = default)
    {
        var normalizedQuery = TeamNameNormalizer.Normalize(query);

        if (string.IsNullOrWhiteSpace(normalizedQuery))
            return null;

        if (!_teamLookup.TryGetValue(normalizedQuery, out var teamDefinition))
            return null;

        var providerTeams = await _footballApiClient.GetPremierLeagueTeamsAsync(cancellationToken);
        var providerTeamLookup = BuildProviderTeamLookup(providerTeams);

        return MapToTeamDto(teamDefinition, providerTeamLookup);
    }

    private static Dictionary<string, TeamAliasDefinition> BuildTeamLookup(
        IReadOnlyCollection<TeamAliasDefinition> supportedTeams)
    {
        var lookup = new Dictionary<string, TeamAliasDefinition>(StringComparer.Ordinal);

        foreach (var team in supportedTeams)
        {
            foreach (var searchTerm in team.GetSearchTerms())
            {
                var normalizedSearchTerm = TeamNameNormalizer.Normalize(searchTerm);

                if (string.IsNullOrWhiteSpace(normalizedSearchTerm))
                    continue;

                if (lookup.TryGetValue(normalizedSearchTerm, out var existingTeam) &&
                    !string.Equals(existingTeam.OfficialName, team.OfficialName, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException($"Team alias '{searchTerm}' is already mapped to '{existingTeam.OfficialName}'.");
                }

                lookup[normalizedSearchTerm] = team;
            }
        }

        return lookup;
    }

    private static Dictionary<string, ApiFootballTeamItem> BuildProviderTeamLookup(
        IReadOnlyCollection<ApiFootballTeamItem> providerTeams)
        => providerTeams
            .Where(team => !string.IsNullOrWhiteSpace(team.Team.Name))
            .GroupBy(team => TeamNameNormalizer.Normalize(team.Team.Name))
            .ToDictionary(
                group => group.Key,
                group => group.First(),
                StringComparer.Ordinal);

    private static TeamDto? MapToTeamDto(
        TeamAliasDefinition teamDefinition,
        IReadOnlyDictionary<string, ApiFootballTeamItem> providerTeamLookup)
    {
        var normalizedProviderName = TeamNameNormalizer.Normalize(teamDefinition.ProviderName);

        if (!providerTeamLookup.TryGetValue(normalizedProviderName, out var providerTeam))
            return null;

        return new TeamDto
        {
            Id = providerTeam.Team.Id,
            Name = teamDefinition.OfficialName,
            ProviderName = providerTeam.Team.Name,
            LogoUrl = providerTeam.Team.Logo
        };
    }
}