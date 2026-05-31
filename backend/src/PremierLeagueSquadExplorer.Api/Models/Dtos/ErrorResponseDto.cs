namespace PremierLeagueSquadExplorer.Api.Models.Dtos;

public sealed class ErrorResponseDto
{
    public string Code { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}