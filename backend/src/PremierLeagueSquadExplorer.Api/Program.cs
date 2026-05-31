using Microsoft.Extensions.Options;
using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Middleware;
using PremierLeagueSquadExplorer.Api.Options;
using PremierLeagueSquadExplorer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

var allowedOrigins = builder.Configuration
    .GetSection("Frontend:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyNames.FrontendDevelopment, policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddOptions<FootballApiOptions>()
    .Bind(builder.Configuration.GetSection(FootballApiOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl), "Football API base URL is required.")
    .Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), "Football API base URL must be valid.")
    .Validate(options => options.LeagueId > 0, "Football API league ID is required.")
    .Validate(options => options.Season > 0, "Football API season is required.")
    .Validate(options => options.MaxPlayerPages > 0, "Football API max player pages must be greater than zero.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey), "Football API key is required.")
    .ValidateOnStart();

builder.Services.AddHttpClient<IFootballApiClient, FootballApiClient>((serviceProvider, httpClient) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<FootballApiOptions>>()
        .Value;

    httpClient.BaseAddress = new Uri(options.BaseUrl);
    httpClient.DefaultRequestHeaders.Add(FootballApiHeaders.ApiKey, options.ApiKey);
});

builder.Services.AddSingleton<ITeamAliasProvider, JsonTeamAliasProvider>();

builder.Services.AddScoped<ITeamResolverService, TeamResolverService>();
builder.Services.AddScoped<ISquadService, SquadService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Premier League Squad Explorer API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyNames.FrontendDevelopment);

app.MapControllers();

app.Run();
