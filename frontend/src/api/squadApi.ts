import { appConfig } from "../config/appConfig";
import type {
  ErrorResponseDto,
  PlayerDto,
  SquadDto,
  TeamDto,
} from "../models/squad";
import { decodeHtml, decodeHtmlOrNull } from "../utils/text";

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(`${appConfig.apiBaseUrl}${path}`);

  if (!response.ok) {
    const error = await readErrorResponse(response);

    throw new ApiError(error.message, response.status, error.code);
  }

  return response.json() as Promise<T>;
}

async function readErrorResponse(
  response: Response,
): Promise<ErrorResponseDto> {
  const fallbackError: ErrorResponseDto = {
    code: "REQUEST_FAILED",
    message: `Request failed with status ${response.status}.`,
  };

  const contentType = response.headers.get("content-type") ?? "";

  if (!contentType.includes("application/json")) {
    return fallbackError;
  }

  try {
    const error = (await response.json()) as Partial<ErrorResponseDto>;

    return {
      code: error.code?.trim() || fallbackError.code,
      message: error.message?.trim() || fallbackError.message,
    };
  } catch (exception) {
    console.warn("Failed to parse backend error response.", exception);

    return {
      code: "INVALID_ERROR_RESPONSE",
      message: fallbackError.message,
    };
  }
}

export async function getTeams(): Promise<TeamDto[]> {
  const teams = await getJson<TeamDto[]>("/api/teams");

  return teams.map(normalizeTeam);
}

export async function getSquad(query: string): Promise<SquadDto> {
  const params = new URLSearchParams({
    query: query.trim(),
  });

  const squad = await getJson<SquadDto>(`/api/squads?${params.toString()}`);

  return normalizeSquad(squad);
}

function normalizeSquad(squad: SquadDto): SquadDto {
  return {
    ...squad,
    team: normalizeTeam(squad.team),
    players: squad.players.map(normalizePlayer),
  };
}

function normalizeTeam(team: TeamDto): TeamDto {
  return {
    ...team,
    name: decodeHtml(team.name).trim(),
    providerName: decodeHtmlOrNull(team.providerName),
  };
}

function normalizePlayer(player: PlayerDto): PlayerDto {
  return {
    ...player,
    firstName: decodeHtml(player.firstName).trim(),
    surname: decodeHtml(player.surname).trim(),
    displayName: decodeHtml(player.displayName).trim(),
    position: decodeHtmlOrNull(player.position),
  };
}

export class ApiError extends Error {
  public readonly status: number;
  public readonly code: string;

  constructor(message: string, status: number, code: string) {
    super(message);

    this.name = "ApiError";
    this.status = status;
    this.code = code;
  }
}
