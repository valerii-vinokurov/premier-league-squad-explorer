using System.Net;

namespace PremierLeagueSquadExplorer.Api.Exceptions;

public sealed class FootballApiException(
    string message,
    HttpStatusCode? statusCode = null,
    Exception? innerException = null) : Exception(message, innerException)
{
    public HttpStatusCode? StatusCode { get; } = statusCode;
}