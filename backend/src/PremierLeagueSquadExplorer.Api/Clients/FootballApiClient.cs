using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;
using PremierLeagueSquadExplorer.Api.Options;

namespace PremierLeagueSquadExplorer.Api.Clients;

public sealed class FootballApiClient : IFootballApiClient
{
    private const int FirstPage = 1;

    private const string TeamsEndpoint = "teams";
    private const string PlayersEndpoint = "players";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;
    private readonly FootballApiOptions _options;

    public FootballApiClient(
        HttpClient httpClient,
        IOptions<FootballApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<IReadOnlyCollection<ApiFootballTeamItem>> GetPremierLeagueTeamsAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await GetFromApiAsync<ApiFootballTeamsResponse>(
            BuildTeamsRequestUrl(),
            apiResponse => apiResponse.Errors,
            cancellationToken);

        return response.Response;
    }

    public async Task<IReadOnlyCollection<ApiFootballPlayerItem>> GetPlayersByTeamAsync(
        int teamId,
        CancellationToken cancellationToken = default)
    {
        var players = new List<ApiFootballPlayerItem>();

        var firstPage = await GetPlayersPageAsync(teamId, FirstPage, cancellationToken);
        players.AddRange(firstPage.Response);

        var totalPages = Math.Max(firstPage.Paging.Total, FirstPage);

        for (var page = FirstPage + 1; page <= totalPages; page++)
        {
            var nextPage = await GetPlayersPageAsync(teamId, page, cancellationToken);
            players.AddRange(nextPage.Response);
        }

        return players;
    }

    private Task<ApiFootballPlayersResponse> GetPlayersPageAsync(
        int teamId,
        int page,
        CancellationToken cancellationToken)
    {
        return GetFromApiAsync<ApiFootballPlayersResponse>(
            BuildPlayersRequestUrl(teamId, page),
            apiResponse => apiResponse.Errors,
            cancellationToken);
    }

    private string BuildTeamsRequestUrl()
    {
        return $"{TeamsEndpoint}?league={_options.LeagueId}&season={_options.Season}";
    }

    private string BuildPlayersRequestUrl(int teamId, int page)
    {
        return $"{PlayersEndpoint}?team={teamId}&league={_options.LeagueId}&season={_options.Season}&page={page}";
    }

    private async Task<TResponse> GetFromApiAsync<TResponse>(
        string requestUrl,
        Func<TResponse, JsonElement> getErrors,
        CancellationToken cancellationToken)
        where TResponse : class
    {
        using var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Football API request failed with status code {(int)response.StatusCode}.");
        }

        var result = await response.Content.ReadFromJsonAsync<TResponse>(
            JsonOptions,
            cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("Football API returned an empty response.");
        }

        ValidateProviderErrors(getErrors(result));

        return result;
    }

    private static void ValidateProviderErrors(JsonElement errors)
    {
        if (HasProviderErrors(errors))
        {
            throw new InvalidOperationException("Football API returned provider errors.");
        }
    }

    private static bool HasProviderErrors(JsonElement errors)
    {
        return errors.ValueKind switch
        {
            JsonValueKind.Object => errors.EnumerateObject().Any(),
            JsonValueKind.Array => errors.GetArrayLength() > 0,
            JsonValueKind.String => !string.IsNullOrWhiteSpace(errors.GetString()),
            _ => false
        };
    }
}