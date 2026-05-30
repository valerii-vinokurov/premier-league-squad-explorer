namespace PremierLeagueSquadExplorer.Api.Models.TeamLookup;

// Represents one supported EPL team mapping.
//
// OfficialName:
// The club name returned by our backend to the frontend.
//
// ProviderName:
// The team name used by API-Football. It may differ from the official club name.
//
// Aliases:
// Additional supported search terms, including short names and club nicknames.
public sealed record TeamAliasDefinition(
    string OfficialName,
    string ProviderName,
    IReadOnlyCollection<string> Aliases)
{
    public IEnumerable<string> GetSearchTerms()
    {
        yield return OfficialName;
        yield return ProviderName;

        foreach (var alias in Aliases)
            yield return alias;
    }
}