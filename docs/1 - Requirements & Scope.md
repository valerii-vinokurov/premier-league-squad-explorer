# Full Stack Tech Assignment

## Requirements and Scope

## Document Purpose

This document defines the initial requirements, MVP scope, assumptions, risks, and limitations for the Full Stack Technical Assignment.

The goal is to clarify what the proof of concept should deliver, what is intentionally out of scope, and which technical risks must be managed during implementation.

## 1. Executive Summary

The assignment is to design and implement a web-based proof of concept that allows a user to search for an English Premier League team from the 2024/25 season and view squad details for the selected team.

The solution will use:

- React for the front-end;
- C# / ASP.NET Core Web API as the middleware layer;
- a third-party football data API as the source of squad and player data;
- GitHub as the public repository;
- GitHub Actions or equivalent tooling for CI/CD demonstration;
- a high-level design document in PDF format.

The MVP will focus on delivering a working end-to-end flow:

1. the user searches for an EPL 2024/25 team;
2. the React front-end calls the C# back-end;
3. the back-end resolves the team and retrieves squad/player data from the external football API;
4. the front-end displays the required player information in a readable UI.

The bonus feature is club nickname search, for example:

```text
The Hammers -> West Ham United
```

## 2. Functional Requirements

The functional requirements are grouped by user flow so they are easier to review and validate.

### 2.1 Team Search

| ID    | Requirement                                         | Acceptance Criteria                                                                                    |
| ----- | --------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| FR-01 | User can search for an EPL team                     | The user can enter a Premier League 2024/25 club name in the React UI.                                 |
| FR-02 | User can search by official club name               | Searching for an official name such as `Manchester United` resolves the correct club.                  |
| FR-03 | User can search by club nickname — bonus            | Supported nicknames resolve to the correct club, for example `The Hammers -> West Ham United`.         |
| FR-04 | User receives useful feedback when no team is found | Unknown names or unsupported nicknames return a clear not-found response and UI message.               |
| FR-05 | Search input is normalized                          | Matching should be case-insensitive and should handle extra spaces and common punctuation differences. |

### 2.2 Squad Retrieval

| ID    | Requirement                                           | Acceptance Criteria                                                                                  |
| ----- | ----------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| FR-06 | User can retrieve squad details for the selected team | After a valid team search, the application returns squad data for the resolved club.                 |
| FR-07 | Squad data is retrieved through the back-end          | The React front-end calls only the C# API, not the external football provider directly.              |
| FR-08 | External provider failures are handled clearly        | Provider unavailability, rate limits, or provider errors produce a clean application error response. |

### 2.3 Player Data Display

| ID    | Requirement                                   | Acceptance Criteria                                               |
| ----- | --------------------------------------------- | ----------------------------------------------------------------- |
| FR-09 | Squad result contains player profile picture  | Each player displays a profile picture when available.            |
| FR-10 | Squad result contains player first name       | Each player displays their first name.                            |
| FR-11 | Squad result contains player surname          | Each player displays their surname / last name.                   |
| FR-12 | Squad result contains player date of birth    | Each player displays their date of birth when available.          |
| FR-13 | Squad result contains player playing position | Each player displays their playing position when available.       |
| FR-14 | Missing player data is handled gracefully     | Missing photos, dates of birth, or positions do not break the UI. |

### 2.4 User Experience

| ID    | Requirement                                   | Acceptance Criteria                                                |
| ----- | --------------------------------------------- | ------------------------------------------------------------------ |
| FR-15 | User can view squad data in a readable web UI | Squad data is displayed in a structured, user-friendly layout.     |
| FR-16 | Loading state is visible                      | The UI shows progress while squad data is being loaded.            |
| FR-17 | Error state is visible                        | The UI shows a clear error message when data cannot be loaded.     |
| FR-18 | Empty / initial state is visible              | The UI clearly guides the user before a search is performed.       |
| FR-19 | UI is responsive                              | The interface works on mobile, tablet, desktop, and large screens. |

## 3. Technical and Delivery Requirements

The technical requirements are grouped by delivery area to avoid mixing implementation, repository, security, and documentation concerns.

### 3.1 Front-end

| ID    | Requirement                                                                            |
| ----- | -------------------------------------------------------------------------------------- |
| TR-01 | The browser-based user interface must be implemented with React.                       |
| TR-02 | The front-end should call the C# back-end API, not the external football API directly. |
| TR-03 | The front-end should display loading, error, empty, and success states.                |
| TR-04 | The front-end should be responsive and usable across common viewport sizes.            |

### 3.2 Back-end and API Integration

| ID    | Requirement                                                                              |
| ----- | ---------------------------------------------------------------------------------------- |
| TR-05 | The middleware layer must be implemented with C# / ASP.NET Core Web API.                 |
| TR-06 | The back-end must integrate with a third-party football data API where possible.         |
| TR-07 | The back-end must expose clean endpoints for team search and squad data.                 |
| TR-08 | Provider-specific response models should be mapped to stable application DTOs.           |
| TR-09 | Provider limitations, rate limits, and incomplete data should be handled and documented. |
| TR-10 | The back-end should use caching where appropriate to reduce repeated provider calls.     |

### 3.3 Repository and Delivery

| ID    | Requirement                                                                                                                |
| ----- | -------------------------------------------------------------------------------------------------------------------------- |
| TR-11 | The solution must be hosted in a public GitHub repository.                                                                 |
| TR-12 | The repository must contain all associated source code, documentation, comments, diagrams, and attempted assignment parts. |
| TR-13 | The solution must include a working proof of concept that demonstrates the main end-to-end flow.                           |
| TR-14 | The repository should demonstrate CI/CD validation using GitHub Actions or equivalent tooling.                             |

### 3.4 Security and Configuration

| ID    | Requirement                                                                                                           |
| ----- | --------------------------------------------------------------------------------------------------------------------- |
| TR-15 | API keys, secrets, and environment-specific configuration must not be committed to GitHub.                            |
| TR-16 | Secrets should be provided through environment variables, user secrets, or local files excluded from version control. |
| TR-17 | The API-Football key must remain server-side and must not be exposed to the React front-end.                          |

### 3.5 Documentation and Review

| ID    | Requirement                                                                                                                                                    |
| ----- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| TR-18 | The repository must include a populated README with setup, run instructions, assumptions, limitations, and future improvements.                                |
| TR-19 | The solution must include a high-level design document in PDF format.                                                                                          |
| TR-20 | Documentation should include diagrams where they help explain the architecture or flow.                                                                        |
| TR-21 | The implementation, trade-offs, technical challenges, limitations, and future improvements should be clear enough to present during the final interview stage. |

## 4. MVP Scope

The MVP is a working web-based proof of concept that allows a user to search for an English Premier League 2024/25 team by official club name and view squad details returned through a C# middleware API.

### In Scope

The MVP includes:

- React-based web UI;
- C# ASP.NET Core Web API middleware;
- integration with a third-party football data API;
- search by official EPL club name;
- retrieval of squad/player data for the selected club;
- display of required player fields:
  - profile picture;
  - first name;
  - surname;
  - date of birth;
  - playing position;
- loading state;
- error state;
- not-found state;
- missing player image fallback;
- public GitHub repository;
- README with local run instructions;
- GitHub Actions CI pipeline or equivalent CI/CD demonstration;
- high-level design PDF.

### Out of Scope for MVP

The MVP does not include:

- user authentication;
- database persistence;
- admin panel;
- manual squad management;
- advanced filtering or sorting;
- full production deployment;
- real-time updates;
- multi-league support;
- historical squad comparison;
- complex production-grade UI/UX design.

## 5. Bonus Scope

The bonus scope is limited to nickname search. It extends the MVP search flow, but it does not introduce a separate user journey or a separate squad endpoint.

### Bonus Feature

The user can enter a supported club nickname and receive squad details for the matching EPL 2024/25 club.

Example:

```text
The Hammers -> West Ham United
```

### In Scope for Bonus

The bonus scope includes:

- nickname-to-club mapping;
- support for the assignment example: `The Hammers -> West Ham United`;
- nickname mappings for all or most EPL 2024/25 clubs where practical;
- input normalization for nickname matching:
  - trim spaces;
  - case-insensitive comparison;
  - handle common punctuation differences;
  - support nicknames with or without `The` where reasonable;
- reuse of the same back-end squad endpoint used by official team names;
- back-end tests for nickname resolution;
- README section explaining nickname search;
- front-end helper text or examples showing nickname search.

### Out of Scope for Bonus

The bonus scope does not include:

- fuzzy search with typo correction;
- AI-based nickname recognition;
- multiple languages for nicknames;
- user-managed nickname dictionary;
- historical nicknames;
- support for clubs outside EPL 2024/25;
- a separate nickname-only endpoint.

## 6. Assumptions

The assumptions are grouped by theme so they can be reviewed as project constraints rather than mixed implementation notes.

### 6.1 Product and Scope Assumptions

| ID   | Assumption                                                            | Impact                                                                                                           |
| ---- | --------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| A-01 | The application is a proof of concept, not a production-ready system. | The solution should demonstrate the main end-to-end flow clearly without unnecessary over-engineering.           |
| A-02 | The core scope is the EPL 2024/25 squad explorer flow.                | Features outside team search and squad display are treated as out of scope unless explicitly required.           |
| A-03 | Nickname search is a bonus feature.                                   | The MVP should not be blocked by nickname search. Bonus work is implemented only after the core flow is working. |
| A-04 | Full production deployment may not be required for the MVP.           | A CI validation pipeline and documented deployment approach may be acceptable unless time allows actual hosting. |

### 6.2 External Provider Assumptions

| ID   | Assumption                                                                               | Impact                                                                                                                             |
| ---- | ---------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| A-05 | EPL 2024/25 squad data will be retrieved from a third-party football API where possible. | Data availability and shape depend on the selected provider.                                                                       |
| A-06 | The external API may have limitations.                                                   | Missing player photos, incomplete data, free-tier restrictions, rate limits, and pagination limits must be handled and documented. |
| A-07 | Squad data freshness depends on the external provider.                                   | The application can only return data that the selected provider makes available.                                                   |
| A-08 | Provider-specific names may differ from official club names.                             | The back-end needs team mapping, normalization, and alias support.                                                                 |
| A-09 | Player profile picture fallback may be required.                                         | The UI should show a placeholder when the provider does not return a valid image.                                                  |

### 6.3 Architecture and Implementation Assumptions

| ID   | Assumption                                                           | Impact                                                                                                  |
| ---- | -------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| A-10 | The back-end should hide third-party API details from the front-end. | API keys, provider models, mapping logic, caching, and error handling stay server-side.                 |
| A-11 | No database is required for the MVP.                                 | Temporary in-memory caching is sufficient for the proof of concept.                                     |
| A-12 | Team search initially supports official EPL club names.              | Provider aliases and bonus nicknames extend the same resolver rather than requiring separate endpoints. |
| A-13 | CI/CD demonstration can be implemented with GitHub Actions.          | GitHub Actions is used for restore, build, test, and front-end build validation.                        |

### 6.4 Documentation and Review Assumptions

| ID   | Assumption                                                   | Impact                                                                                                       |
| ---- | ------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------ |
| A-14 | Documentation is part of the delivery.                       | README and high-level design documentation should reduce reviewer friction.                                  |
| A-15 | Trade-offs and limitations should be documented clearly.     | Known provider limitations, architectural decisions, and future improvements should be visible to reviewers. |
| A-16 | The solution should be presentable in a follow-up interview. | Architecture, implementation choices, risks, and trade-offs should be easy to explain.                       |

## 7. Risks and Limitations

| ID   | Risk / Limitation                                                    | Mitigation                                                                                           |
| ---- | -------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| R-01 | External football API may not provide complete data.                 | Map available data defensively and document provider gaps.                                           |
| R-02 | EPL 2024/25 squad data may be limited or unavailable.                | Validate provider behavior early and document known limitations.                                     |
| R-03 | Player profile pictures may be missing.                              | Show a placeholder image in the UI.                                                                  |
| R-04 | API rate limits may affect testing and demo.                         | Use caching and avoid unnecessary repeated provider calls.                                           |
| R-05 | External API availability can affect the application.                | Return clean error responses and document provider dependency.                                       |
| R-06 | Provider data model may not match assignment fields exactly.         | Normalize and map provider responses into stable DTOs.                                               |
| R-07 | Nickname search can be ambiguous.                                    | Support a documented alias dictionary rather than fuzzy or AI-based matching.                        |
| R-08 | No database persistence in MVP.                                      | Use in-memory caching and document persistence as a future improvement.                              |
| R-09 | CI/CD may demonstrate validation rather than full deployment.        | Provide a reliable GitHub Actions pipeline and document deployment as out of scope.                  |
| R-10 | Time constraint may limit polish and optional features.              | Complete MVP first, then add bonus and polish only when safe.                                        |
| R-11 | Front-end design may remain functional rather than production-grade. | Prioritize clarity, responsive behavior, and a stable demo flow.                                     |
| R-12 | Test coverage may focus on critical back-end logic.                  | Cover resolver, nickname mapping, data mapping, caching, provider errors, and controller validation. |
| R-13 | Data accuracy depends on the provider.                               | Document known data gaps and provider assumptions.                                                   |
| R-14 | Secrets must be handled carefully.                                   | Use user secrets, environment variables, and ignored local config files.                             |
| R-15 | Demo reliability can be affected by live provider calls.             | Use caching, screenshots, documented limitations, and clear fallback behavior.                       |

## 8. Scope Control Strategy

The implementation should prioritize the following order:

1. validate the external football API;
2. implement the back-end API integration;
3. implement the React UI;
4. add error handling and caching;
5. add tests for critical back-end logic;
6. document the solution and local setup;
7. add CI/CD validation;
8. complete the high-level design PDF;
9. implement or finalize bonus nickname search if the core flow is stable;
10. polish UI and demo flow.

The core MVP should not be blocked by optional enhancements. If time becomes limited, optional improvements should be documented as future work instead of increasing delivery risk.

## 9. Summary

The defined MVP is a focused, web-based proof of concept that demonstrates the required full-stack flow:

```text
React front-end -> C# middleware -> third-party football API -> squad/player data displayed in the browser
```

The solution should prioritize:

- correctness;
- clarity;
- secure API usage;
- documented trade-offs;
- stable demo flow;
- reviewer-friendly documentation.

Bonus nickname search should extend the same team resolution flow without introducing unnecessary complexity.

The key technical risks are external API data completeness, player photo availability, rate limits, provider-specific naming, and demo reliability. These risks are managed through provider validation, back-end normalization, caching, fallback UI behavior, tests, and clear documentation.
