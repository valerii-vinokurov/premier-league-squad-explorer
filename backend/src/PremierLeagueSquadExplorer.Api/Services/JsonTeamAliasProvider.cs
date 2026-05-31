using System.Text.Json;
using PremierLeagueSquadExplorer.Api.Models.TeamLookup;

namespace PremierLeagueSquadExplorer.Api.Services;

public sealed class JsonTeamAliasProvider : ITeamAliasProvider
{
    private const string TeamAliasesFilePath = "Data/team-aliases.json";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IReadOnlyCollection<TeamAliasDefinition> _supportedTeams;

    public JsonTeamAliasProvider(IHostEnvironment environment)
    {
        var filePath = Path.Combine(environment.ContentRootPath, TeamAliasesFilePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Team aliases file was not found.", filePath);

        var json = File.ReadAllText(filePath);

        _supportedTeams = JsonSerializer.Deserialize<IReadOnlyCollection<TeamAliasDefinition>>(json, JsonOptions) ?? [];

        Validate(_supportedTeams);
    }

    public IReadOnlyCollection<TeamAliasDefinition> GetSupportedTeams()
        => _supportedTeams;

    private static void Validate(IReadOnlyCollection<TeamAliasDefinition> supportedTeams)
    {
        if (supportedTeams.Count == 0)
            throw new InvalidOperationException("Team aliases file is empty.");

        foreach (var team in supportedTeams)
        {
            if (string.IsNullOrWhiteSpace(team.OfficialName))
                throw new InvalidOperationException("Team official name is required.");

            if (string.IsNullOrWhiteSpace(team.ProviderName))
                throw new InvalidOperationException($"Provider name is required for team '{team.OfficialName}'.");

            if (team.Aliases is null)
                throw new InvalidOperationException($"Aliases collection is required for team '{team.OfficialName}'.");
        }
    }
}