using System.Net;
using System.Text.Json;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Exceptions;
using PremierLeagueSquadExplorer.Api.Models.Dtos;

namespace PremierLeagueSquadExplorer.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Client disconnected or request was cancelled.
            // Let ASP.NET Core handle the aborted request.
            throw;
        }
        catch (FootballApiException exception)
        {
            await HandleFootballApiExceptionAsync(context, exception);
        }
        catch (JsonException exception)
        {
            _logger.LogError(exception, "External football API response could not be parsed.");

            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status502BadGateway,
                ErrorCodes.FootballApiError,
                "Squad data could not be loaded from the football data provider.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled backend exception.");

            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                ErrorCodes.UnexpectedError,
                "An unexpected error occurred.");
        }
    }

    private async Task HandleFootballApiExceptionAsync(HttpContext context, FootballApiException exception)
    {
        _logger.LogError(
            exception,
            "Football API request failed. Provider status code: {StatusCode}. Provider errors: {ProviderErrors}",
            exception.StatusCode,
            exception.ProviderErrors);

        if (exception.StatusCode == HttpStatusCode.TooManyRequests)
        {
            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status503ServiceUnavailable,
                ErrorCodes.RateLimited,
                "Football data provider rate limit was reached. Please try again later.");

            return;
        }

        await WriteErrorResponseAsync(
            context,
            StatusCodes.Status502BadGateway,
            ErrorCodes.FootballApiError,
            "Squad data could not be loaded from the football data provider.");
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string code,
        string message)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponseDto
        {
            Code = code,
            Message = message
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}