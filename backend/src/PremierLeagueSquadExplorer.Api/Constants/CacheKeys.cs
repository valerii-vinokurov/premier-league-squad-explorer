namespace PremierLeagueSquadExplorer.Api.Constants;

public static class CacheKeys
{
    public static string PremierLeagueTeams(int leagueId, int season)
        => $"teams:league:{leagueId}:season:{season}";

    public static string SquadByTeam(int teamId, int leagueId, int season)
        => $"squad:team:{teamId}:league:{leagueId}:season:{season}";
}