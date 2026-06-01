# Premier League Squad Explorer

![CI](https://github.com/valerii-vinokurov/premier-league-squad-explorer/actions/workflows/ci.yml/badge.svg)

Premier League Squad Explorer is a full-stack proof of concept that allows users to search for English Premier League clubs and view squad player information.

The application consists of:

- ASP.NET Core Web API back-end middleware;
- React + TypeScript front-end;
- API-Football integration;
- in-memory caching;
- nickname-based team search;
- automated back-end tests;
- GitHub Actions CI pipeline.

## Features

- Search Premier League clubs by official name.
- Search clubs by supported nickname, for example `The Hammers`.
- Resolve user-friendly club names to API-Football provider names.
- Display squad player information:
  - profile picture;
  - first name;
  - surname;
  - date of birth;
  - playing position.

- Handle loading, empty, error, and missing-data states.
- Use in-memory caching to reduce repeated API-Football requests.
- Provide Swagger/OpenAPI documentation for the back-end.
- Validate the project through GitHub Actions.

## Tech Stack

### Back-end

- .NET 9
- ASP.NET Core Web API
- HttpClientFactory
- Options pattern
- MemoryCache
- Swagger/OpenAPI
- xUnit

### Front-end

- React
- TypeScript
- Vite
- Bootstrap
- Bootstrap Icons
- SCSS

### External Provider

- API-Football

## Project Structure

```text
backend/
  src/
    PremierLeagueSquadExplorer.Api/
      Clients/
      Controllers/
      Data/
      Exceptions/
      Middleware/
      Models/
      Options/
      Services/

  tests/
    PremierLeagueSquadExplorer.Api.Tests/
      Clients/
      Controllers/
      Services/
      TestData/
      Fakers/

frontend/
  src/
    api/
    components/
    config/
    hooks/
    models/
    pages/
    styles/
    utils/

.github/
  workflows/
    ci.yml
```

## Architecture Overview

The React front-end communicates only with the ASP.NET Core back-end.

![alt text](High-Level%20Architecture.png)

The API key is kept on the back-end and is never exposed to the front-end.

The back-end is responsible for:

- loading Premier League team data from API-Football;
- resolving official names and supported nicknames;
- loading squad player data;
- normalizing API-Football responses into clean DTOs;
- caching team and squad data;
- handling provider errors and rate limits.

The front-end is responsible for:

- providing a clean search experience;
- displaying squad data;
- showing loading, empty, and error states;
- rendering missing player photos with a fallback placeholder;
- adapting the UI for mobile, tablet, desktop, and large screens.

## Back-end Setup

### Prerequisites

- .NET 9 SDK
- API-Football API key

### Configure API-Football API key

From the repository root, run:

```bash
dotnet user-secrets set "FootballApi:ApiKey" "<YOUR_API_FOOTBALL_KEY>" --project backend/src/PremierLeagueSquadExplorer.Api
```

The API key should not be committed to source control.

### Run the back-end

```bash
dotnet run --project backend/src/PremierLeagueSquadExplorer.Api
```

By default, the API runs on:

```text
https://localhost:7082
http://localhost:5136
```

Swagger UI is available at:

```text
https://localhost:7082/swagger
```

## Front-end Setup

### Prerequisites

- Node.js 22
- npm

### Configure front-end API URL

Create a local environment file:

```bash
cd frontend
cp .env.example .env.local
```

Example `.env.local`:

```env
VITE_API_BASE_URL=https://localhost:7082
```

If the browser blocks the local HTTPS development certificate, use the HTTP backend URL instead:

```env
VITE_API_BASE_URL=http://localhost:5136
```

After changing `.env.local`, restart the Vite dev server.

### Run the front-end

```bash
cd frontend
npm ci
npm run dev
```

The front-end runs on:

```text
http://localhost:5173
```

## API Endpoints

### Get supported teams

```http
GET /api/teams
```

Example response:

```json
[
  {
    "id": 48,
    "name": "West Ham United",
    "providerName": "West Ham",
    "logoUrl": "https://..."
  }
]
```

### Get squad by team name or nickname

```http
GET /api/squads?query=The%20Hammers
```

Example response:

```json
{
  "team": {
    "id": 48,
    "name": "West Ham United",
    "providerName": "West Ham",
    "logoUrl": "https://..."
  },
  "players": [
    {
      "id": 123,
      "firstName": "Example",
      "surname": "Player",
      "dateOfBirth": "2000-01-01",
      "position": "Midfielder",
      "profilePictureUrl": "https://...",
      "displayName": "E. Player"
    }
  ],
  "metadata": {
    "leagueId": 39,
    "season": 2024,
    "source": "API-Football",
    "cached": false
  }
}
```

## Nickname Search

The application supports searching by official club names and selected common nicknames.

Example:

```text
The Hammers -> West Ham United
```

Nickname resolution is configured through the JSON-based team alias dictionary:

```text
backend/src/PremierLeagueSquadExplorer.Api/Data/team-aliases.json
```

The resolver normalizes user input before matching. This means differences in casing, extra spaces, punctuation, and common name variants can be handled consistently.

Examples:

```text
The Hammers
hammers
  THE   HAMMERS
West Ham
West Ham United
```

All of the above resolve to:

```text
West Ham United
```

## Caching

The back-end uses in-memory caching to reduce repeated external API calls.

Cached areas include:

- supported team resolution data;
- squad results per team and season.

This helps protect the application from unnecessary API-Football requests during local testing and demo usage.

## Error Handling

The back-end has centralized exception handling middleware.

Handled scenarios include:

- invalid search input;
- team not found;
- API-Football provider failures;
- API-Football rate limits;
- unexpected application errors.

The front-end receives clean user-friendly error responses and does not display internal provider details.

## Provider Assumptions and Limitations

This project uses API-Football as the external football data provider.

Known assumptions and limitations:

- The application targets the English Premier League.
- The configured league ID is `39`.
- The configured season is `2024`.
- API-Football free plans may apply request limits and endpoint restrictions.
- API-Football free plans limit the `/players` endpoint to a maximum page value of `3`.
- The back-end respects provider limitations through configuration and returns the available player data.
- Provider data may include wider season player records rather than a compact official first-team squad.
- Missing player photo, date of birth, or position values are handled defensively.

## Testing

Run all back-end tests:

```bash
dotnet test
```

The test suite covers:

- team name normalization;
- official team name resolution;
- nickname mapping;
- invalid input;
- unknown team handling;
- squad mapping;
- missing player data;
- missing player photo handling;
- player de-duplication;
- caching behavior;
- provider/API failure behavior;
- controller-level error responses.

## Front-end Validation

The React UI was manually validated for:

- official club name search;
- nickname search;
- loading state;
- empty state;
- error state;
- missing or broken player image fallback;
- mobile layout;
- tablet layout;
- desktop layout;
- large screen layout.

Automated front-end tests are intentionally out of scope for this proof of concept.

## CI/CD

The repository uses a lightweight GitHub Actions pipeline to validate the project on every push and pull request to `master`.

The pipeline performs the following checks:

- restores back-end dependencies;
- builds the ASP.NET Core back-end;
- runs the back-end test suite;
- installs front-end dependencies with `npm ci`;
- builds the React front-end with Vite.

The workflow also supports manual runs through the GitHub Actions UI.

Deployment is intentionally out of scope for this proof of concept. The pipeline focuses on restore, build, test, and front-end build validation to keep the submission simple and reliable.

## Local Validation Commands

From the repository root:

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

Front-end validation:

```bash
cd frontend
npm ci
npm run build
```

## Future Improvements

Potential improvements if the project were extended:

- add deployment to a cloud environment;
- add end-to-end tests;
- add front-end component tests;
- add persistent distributed caching;
- add structured logging and correlation IDs;
- add retry/backoff policies for external API calls;
- add more detailed provider limitation handling;
- add player sorting and filtering;
- add team badges and richer squad metadata;
- add observability dashboards;
- support additional leagues and seasons.

## Notes for Reviewers

This project intentionally keeps the implementation focused on the assignment goals:

- clean full-stack flow;
- readable architecture;
- stable API responses;
- provider error handling;
- responsive UI;
- test coverage for core back-end logic;
- simple CI validation.

The solution is designed as a proof of concept rather than a production deployment.
