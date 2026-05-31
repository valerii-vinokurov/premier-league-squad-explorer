using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;

namespace PremierLeagueSquadExplorer.Api.Tests.TestData;

internal static class ApiFootballTestData
{
    internal static IReadOnlyCollection<ApiFootballTeamItem> CreateWestHamProviderTeams()
        =>
        [
            new ApiFootballTeamItem
            {
                Team = new ApiFootballTeam
                {
                    Id = 48,
                    Name = "West Ham",
                    Logo = "https://example.test/west-ham.png"
                }
            }
        ];

    internal static ApiFootballPlayerItem CreatePlayer(
        int id = 123,
        string firstName = "Test",
        string lastName = "Player",
        string? dateOfBirth = "2000-01-01",
        string? position = "Midfielder",
        string? photo = "https://example.test/player.png")
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
                        Date = dateOfBirth
                    },
                    Photo = photo
                },
                Statistics =
                [
                    new ApiFootballStatistic
                    {
                        Games = new ApiFootballGames
                        {
                            Position = position
                        }
                    }
                ]
            };
}