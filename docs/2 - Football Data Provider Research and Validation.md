# Football Data Provider Research and Validation

## Full Stack Tech Assignment

## Document Purpose

This document explains the research, validation, and provider selection process for the football data API used in the Full Stack Technical Assignment.

The goal was to identify a third-party football data provider that can support the proof of concept by returning English Premier League 2024/25 team and player data, including the required player fields:

- profile picture;
- first name;
- surname;
- date of birth;
- playing position.

The research also considers access speed, free-tier limitations, response quality, fallback options, implementation risks, and practical delivery constraints.

---

## 1. Executive Summary

The selected primary provider for the proof of concept is **API-Football / API-Sports**.

This decision was made after comparing several football data providers and validating API-Football through real API requests.

The validation confirmed that API-Football can provide:

- EPL 2024/25 team list;
- provider-specific team IDs;
- season-specific player data;
- player profile pictures;
- first name;
- surname;
- date of birth;
- playing position.

The selected implementation uses:

- `GET /teams?league=39&season=2024` for EPL 2024/25 team lookup;
- `GET /players?team={teamId}&league=39&season=2024&page={page}` for player data.

The `/players` endpoint is preferred over `/players/squads` because it provides the fields required by the assignment, including date of birth and separate first name / surname fields.

Known limitations include:

- API-Football free-tier request limits;
- API-Football free-plan `/players` page limit;
- paginated player responses;
- provider-specific team names;
- broader season player records instead of a compact current squad list;
- occasional missing player data.

These limitations are acceptable for the proof of concept when handled through:

- backend provider-aware pagination limits;
- in-memory caching;
- defensive field mapping;
- team alias normalization;
- fallback UI values;
- clear README and high-level design documentation.

---

## 2. Provider Selection Criteria

A suitable provider should support the assignment requirements and be practical for a time-boxed proof of concept.

The provider should ideally:

- support EPL 2024/25 team data;
- provide stable team IDs;
- provide squad or player data for a selected club;
- provide player profile pictures;
- provide first name and surname separately;
- provide date of birth;
- provide playing position;
- support a reasonable development and demo workflow;
- have understandable access, pricing, and rate-limit rules;
- allow API keys to be handled securely through backend configuration.

---

## 3. Provider Shortlist

### 3.1 Sportmonks Football API

Sportmonks appears to be the strongest candidate from a data-model perspective.

It provides football-specific resources for teams, squads, players, positions, and related entities. The model appears well aligned with the assignment because player records can include fields such as first name, last name, date of birth, image path, and position-related data.

#### Strengths

- Strong football-specific data model.
- Supports teams, players, squads, positions, and related includes.
- Player model appears to support:
  - first name;
  - last name;
  - date of birth;
  - image path.
- Squad model appears to support team/player/position relationships.
- Good technical fit for a clean domain model.

#### Limitations / Risks

- Premier League data may not be available in the forever-free plan.
- EPL access may require a paid plan or trial.
- This creates access and timing risk for a short assignment.

#### Assessment

Sportmonks is a strong secondary candidate. It may be the best technical provider if access is available quickly, but the potential plan/trial dependency makes it less practical as the first provider for this proof of concept.

---

### 3.2 API-Football / API-Sports

API-Football is a practical provider for a proof of concept. It provides football data through documented endpoints and includes team lookup, squad, and player endpoints.

#### Strengths

- Supports team lookup by league and season.
- Provides player and squad-related endpoints.
- Provides a free plan suitable for initial testing.
- Can be integrated quickly.
- Real API validation confirmed that the required fields are available through the season-specific player endpoint.

#### Limitations / Risks

- Free-tier usage is limited.
- The `/players` endpoint is paginated.
- Free plans limit the `/players` endpoint to a maximum page value of `3`.
- The `/players/squads` endpoint does not provide all required fields.
- Provider team names may differ from official club names.
- Backend caching and normalization are required.

#### Assessment

API-Football is the best primary provider for this proof of concept because it offers the best balance between accessibility, implementation speed, and validated data coverage.

---

### 3.3 football-data.org

football-data.org is simple and well documented. It can provide team and squad metadata but appears weaker for this assignment because player profile pictures are not available in the expected squad/player response.

#### Strengths

- Simple API.
- Good documentation.
- Can provide team and squad metadata.
- Player data can include name, date of birth, and position.

#### Limitations / Risks

- Player profile pictures are not available in the expected response.
- Does not fully satisfy the assignment requirements on its own.

#### Assessment

football-data.org is a useful fallback for squad metadata, date of birth, and position, but it should not be the primary provider unless combined with placeholder images or another image source.

---

### 3.4 TheSportsDB

TheSportsDB is easy to test and can return player image fields. However, it is less reliable for season-specific squad accuracy.

#### Strengths

- Easy to test.
- Can return player image fields.
- Useful as an emergency or demo fallback.

#### Limitations / Risks

- Data may not be strictly season-specific.
- Response may include non-player staff.
- Data quality and completeness may vary.
- Crowd-sourced data may be less reliable for accurate squad validation.

#### Assessment

TheSportsDB should be treated only as an emergency/demo fallback, not as the main provider.

---

## 4. Initial Provider Decision

The selected provider strategy is:

- **Primary provider:** API-Football / API-Sports
- **Secondary provider:** Sportmonks Football API
- **Fallback provider:** football-data.org with placeholder player images
- **Emergency/demo fallback:** TheSportsDB

The main reason for selecting API-Football first is practicality.

For this assignment, the immediate priority is to implement a working end-to-end flow:

1. identify an EPL 2024/25 team;
2. resolve it to a provider-specific team ID;
3. retrieve player data for that team;
4. map the data into the required response format;
5. display the squad in the React frontend through the C# middleware API.

API-Football provides the best balance for this proof of concept because it is accessible, testable, and supports the necessary team/player data through real validated endpoints.

---

## 5. EPL 2024/25 Team Coverage Validation

### Purpose

The first validation step was to confirm whether the selected provider can return the English Premier League teams for the 2024/25 season.

This is required because the user must be able to search for an EPL 2024/25 team and retrieve squad details for that team.

### Request

```http
GET /teams?league=39&season=2024
```

Where:

- `league=39` represents the Premier League;
- `season=2024` represents the 2024/25 season.

### Result

The request successfully returned 20 Premier League teams.

Key response details:

- `errors`: empty;
- `results`: 20;
- `paging.current`: 1;
- `paging.total`: 1.

Each team record includes:

- team ID;
- team name;
- team code;
- country;
- founded year;
- team logo;
- venue details.

### Assessment

EPL 2024/25 team coverage is confirmed.

API-Football can be used to retrieve the team list and provider-specific team IDs.

### Important Note

Some team names are returned in shortened provider-specific format.

Examples:

- `West Ham` instead of `West Ham United`;
- `Tottenham` instead of `Tottenham Hotspur`;
- `Wolves` instead of `Wolverhampton Wanderers`;
- `Brighton` instead of `Brighton & Hove Albion`;
- `Bournemouth` instead of `AFC Bournemouth`.

### Decision

The backend must include team name normalization and alias mapping.

This is also useful for the bonus nickname search feature.

---

## 6. Squad Endpoint Validation

### Purpose

The second validation step was to check whether API-Football can return squad-like data for a selected EPL team.

West Ham was selected as the test team because it is also useful for validating the bonus nickname scenario:

```text
The Hammers -> West Ham United
```

### Request

```http
GET /players/squads?team=48
```

Where:

- `team=48` represents West Ham.

### Result

The request successfully returned a squad response for West Ham.

Key response details:

- `errors`: empty;
- `results`: 1;
- returned players: 27.

Each player record includes:

- player ID;
- display name;
- age;
- shirt number;
- playing position;
- photo URL.

### Assessment

The `/players/squads` endpoint is useful for a compact squad-like response and confirms that API-Football can provide player photos and positions.

However, this endpoint does not provide all fields required by the assignment.

Missing or insufficient fields:

- exact date of birth;
- separate first name;
- separate surname.

The endpoint returns display names such as `A. Areola` or `J. Bowen`, which are not sufficient to reliably satisfy the required first name and surname fields.

### Decision

The `/players/squads` endpoint should not be used as the main data source for the final proof of concept response.

It may be used as a reference endpoint, but the main implementation should use the season-specific `/players` endpoint.

---

## 7. Season-Specific Player Endpoint Validation

### Purpose

The third validation step was to check whether API-Football can provide the exact player fields required by the assignment.

### Request

```http
GET /players?team=48&league=39&season=2024&page={page}
```

Additional pagination requests were made:

- `page=1`;
- `page=2`;
- `page=3`.

### Result

The endpoint successfully returned season-specific player records for West Ham in the 2024 Premier League season.

Pagination result:

- page 1: 20 records;
- page 2: 20 records;
- page 3: 11 records;
- total returned records: 51;
- `errors`: empty;
- `paging.total`: 3.

Each player record includes the required fields:

- `player.firstname`;
- `player.lastname`;
- `player.birth.date`;
- `player.photo`;
- `statistics[].games.position`.

### Required Field Mapping

| Assignment field | API-Football field             |
| ---------------- | ------------------------------ |
| Profile picture  | `player.photo`                 |
| First name       | `player.firstname`             |
| Surname          | `player.lastname`              |
| Date of birth    | `player.birth.date`            |
| Playing position | `statistics[0].games.position` |

### Assessment

The `/players` endpoint satisfies the required player data fields for this proof of concept.

It is more suitable than `/players/squads` because it provides date of birth and separate first name / surname fields.

### Decision

Use `/players?team={teamId}&league=39&season=2024&page={page}` as the primary player data endpoint.

---

## 8. Recommended Integration Approach

The recommended backend integration approach is:

1. Use `/teams?league=39&season=2024` to retrieve EPL 2024/25 teams and provider-specific team IDs.
2. Normalize user input and resolve the selected club to a provider-specific team ID.
3. Use `/players?team={teamId}&league=39&season=2024&page={page}` to retrieve player records.
4. Read `paging.total` from the first player response.
5. Request additional pages only up to the configured provider limit.
6. Merge all loaded player records.
7. De-duplicate player records by provider player ID.
8. Map the provider response into internal DTOs.
9. Return a clean API response to the React frontend.

Recommended internal DTO fields:

- `profilePictureUrl`;
- `firstName`;
- `surname`;
- `dateOfBirth`;
- `position`;
- `displayName`.

The React frontend should not know about API-Football-specific response models.

---

## 9. Implementation Considerations

### 9.1 Pagination and Free-Plan Page Limit

The `/players` endpoint is paginated.

The backend should:

1. request page 1;
2. read `paging.total`;
3. calculate the number of pages to load using the configured provider limit;
4. request pages 2 through the allowed maximum page;
5. merge all loaded player records;
6. map the merged result into the application DTO.

API-Football free plans limit the `/players` endpoint to a maximum page value of `3`.

Because of this, the implementation should include a configurable setting such as:

```text
FootballApi:MaxPlayerPages = 3
```

This prevents the backend from requesting unsupported pages such as `page=4`, which can result in a provider error.

Example provider error:

```json
{
  "plan": "Free plans are limited to a maximum value of 3 for the Page parameter"
}
```

### 9.2 Data Volume

For West Ham, `/players/squads` returned 27 players, while the season-specific `/players` endpoint returned 51 player records.

This suggests that `/players` returns broader season player records rather than only a compact current squad list.

The returned list may include:

- current squad players;
- players registered during the season;
- players with zero appearances;
- players who may no longer be part of the current squad.

For this proof of concept, this limitation is acceptable because the endpoint is season-specific and provides all required player fields.

This limitation should be documented in the README and high-level design document.

### 9.3 Team Name Normalization

API-Football may return shortened team names.

The backend should support:

- official club names;
- provider-specific names;
- common aliases;
- bonus nicknames.

Example mappings:

- `West Ham United` -> `West Ham`;
- `Tottenham Hotspur` -> `Tottenham`;
- `Wolverhampton Wanderers` -> `Wolves`;
- `Brighton & Hove Albion` -> `Brighton`;
- `AFC Bournemouth` -> `Bournemouth`;
- `The Hammers` -> `West Ham United`.

This mapping should be implemented in the backend, not in the frontend.

### 9.4 Caching

The API-Football free tier has request limits, so caching is important for development, testing, and demo reliability.

Recommended cache entries:

- EPL 2024/25 team list;
- resolved normalized team names;
- player data by team ID and season.

For the proof of concept, in-memory caching is sufficient.

A database or distributed cache is not required.

### 9.5 Secure API Key Handling

The API key must be treated as a secret.

The key must not be committed to GitHub or included in documentation examples.

Recommended local configuration:

```bash
dotnet user-secrets set "FootballApi:ApiKey" "<YOUR_API_FOOTBALL_KEY>" --project backend/src/PremierLeagueSquadExplorer.Api
```

For environment-based configuration, the equivalent ASP.NET Core environment variable format can be used:

```text
FootballApi__ApiKey=<YOUR_API_FOOTBALL_KEY>
```

The repository should include example configuration without real secrets.

---

## 10. Fallback Strategy for Incomplete API Data

The application depends on a third-party API, so the backend should not assume that every field will always be present.

The implementation should handle missing data defensively and return a consistent response to the frontend.

### Missing Player Photo

If `player.photo` is missing, empty, or fails to load:

- display a local placeholder avatar;
- keep the player visible in the squad list;
- do not fail the whole response.

### Missing Date of Birth

If `player.birth.date` is missing:

- return `null`;
- display `Unknown` in the UI;
- do not derive date of birth from age.

Age is not precise enough and may become inaccurate depending on the current date.

### Missing First Name or Surname

If `player.firstname` or `player.lastname` is missing:

- use the available field if one exists;
- use `player.name` as a display fallback;
- avoid unreliable parsing of abbreviated names.

Recommended mapping:

```text
firstName = player.firstname ?? ""
surname = player.lastname ?? ""
displayName = player.name
```

### Missing Playing Position

If `statistics[].games.position` is missing:

- return `Unknown`;
- keep the player visible;
- log the missing value for debugging if needed.

### API Failure or Rate Limit

If API-Football is unavailable, rate-limited, or returns an error:

- return cached data if available;
- otherwise return a clear backend error response;
- show a user-friendly frontend message;
- do not expose provider-specific technical details to the user.

Suggested user message:

```text
Squad data is temporarily unavailable. Please try again later.
```

---

## 11. Secondary Provider Strategy

If API-Football becomes unusable, fallback options are listed below.

### football-data.org

Use for:

- team list;
- squad metadata;
- player name;
- date of birth;
- position.

Limitation:

- player profile pictures are not available in the expected player model.

Fallback behavior:

- use football-data.org for player metadata;
- use placeholder images for player photos;
- document the limitation clearly.

### TheSportsDB

Use only as an emergency/demo fallback.

Use for:

- basic player information;
- player image fields where available.

Limitations:

- data may not be strictly season-specific;
- response may include non-player staff;
- data quality may be inconsistent.

### Sportmonks

Use as a secondary professional provider if access is available.

Use for:

- stronger player and squad modelling;
- player images;
- date of birth;
- positions.

Limitation:

- Premier League access may require trial or paid-plan setup.

---

## 12. Documented API Limitations

API-Football is suitable as the primary provider for the proof of concept, but the implementation must account for the following limitations.

### 12.1 `/players/squads` Does Not Provide All Required Fields

The `/players/squads` endpoint provides a compact squad-like response, but it does not provide exact date of birth or separate first name / surname fields.

**Decision:** use `/players` as the primary player data endpoint.

### 12.2 `/players` Endpoint Is Paginated

The `/players` endpoint can return multiple pages.

**Decision:** the backend API client must implement pagination and merge loaded pages before returning the final squad response.

### 12.3 Free Plans Limit Player Page Values

API-Football free plans limit the `/players` endpoint to a maximum `page` value of `3`.

**Decision:** the backend should use a configurable `MaxPlayerPages` setting and must not request pages beyond the configured limit.

### 12.4 `/players` May Return Broader Season Player Records

The `/players` endpoint may return more records than a compact current squad list.

**Decision:** accept this limitation for the proof of concept because the endpoint is season-specific and provides all required fields. Document this behavior clearly in the README and high-level design document.

### 12.5 Provider Team Names May Differ From Official Club Names

Provider names may be shortened or different from official names.

**Decision:** implement backend normalization and alias mapping.

### 12.6 Some Player Fields Can Still Be Missing

Even though tested responses include the required fields, the application should not assume complete data for every player.

**Decision:** use defensive mapping and frontend fallback values.

### 12.7 Free-Tier Request Limits Require Caching

The free tier has limited request capacity.

**Decision:** use in-memory caching for the proof of concept.

### 12.8 External API Availability Can Affect Demo Reliability

The application depends on a third-party API.

**Decision:** use cached data when possible and return user-friendly errors when live data cannot be loaded.

### 12.9 API Key Must Be Treated as a Secret

The API key must not be committed or exposed.

**Decision:** read the API key from user secrets, environment variables, or local configuration excluded from version control.

---

## 13. Final Decision

API-Football / API-Sports is selected as the primary football data provider for the proof of concept.

It successfully supports:

- EPL 2024/25 team lookup;
- provider-specific team IDs;
- season-specific player data;
- player profile pictures;
- first name;
- surname;
- date of birth;
- playing position.

The selected implementation should use:

- `/teams?league=39&season=2024` for teams;
- `/players?team={teamId}&league=39&season=2024&page={page}` for player data.

The implementation must include:

- provider-aware pagination handling;
- free-plan page limit protection;
- team name normalization;
- defensive field mapping;
- placeholder image fallback;
- user-friendly error handling;
- in-memory caching;
- secure API key configuration.

Overall, API-Football is suitable for the proof of concept with documented limitations and appropriate backend safeguards.
