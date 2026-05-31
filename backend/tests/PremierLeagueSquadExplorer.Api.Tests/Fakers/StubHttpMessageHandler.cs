using System.Net;

namespace PremierLeagueSquadExplorer.Api.Tests.Fakers;

internal sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler = handler;

    public List<Uri> RequestedUris { get; } = [];

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.RequestUri is not null)
        {
            RequestedUris.Add(request.RequestUri);
        }

        return Task.FromResult(_handler(request));
    }

    public static HttpResponseMessage Json(string content)
        => new(HttpStatusCode.OK)
        {
            Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
        };
}