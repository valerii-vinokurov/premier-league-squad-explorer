namespace PremierLeagueSquadExplorer.Api.Models.Dtos;

public sealed class PlayerDto
{
    public int Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string Surname { get; init; } = string.Empty;

    public string? DateOfBirth { get; init; }

    public string Position { get; init; } = "Unknown";

    public string? ProfilePictureUrl { get; init; }

    public string DisplayName { get; init; } = string.Empty;
}