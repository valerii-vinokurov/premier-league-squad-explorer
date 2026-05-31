import type {
  ErrorResponseDto,
  PlayerDto,
  SquadDto,
  TeamDto,
} from "../models/squad";
import { decodeHtml, decodeHtmlOrNull } from "../utils/text";

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7082";

async function getJson<T>(url: string): Promise<T> {
  const response = await fetch(url);

  if (!response.ok) {
    let errorMessage = "Request failed. Please try again.";
    let errorCode = "REQUEST_FAILED";

    try {
      const error = (await response.json()) as Partial<ErrorResponseDto>;

      if (error.message) {
        errorMessage = error.message;
      }

      if (error.code) {
        errorCode = error.code;
      }
    } catch {
      // If the backend does not return JSON, keep the default user-friendly message.
    }

    throw new ApiError(errorMessage, response.status, errorCode);
  }

  return response.json() as Promise<T>;
}

export async function getTeams(): Promise<TeamDto[]> {
  const teams = await getJson<TeamDto[]>(`${API_BASE_URL}/api/teams`);

  return teams.map(normalizeTeam);
}

export async function getSquad(query: string): Promise<SquadDto> {
  const encodedQuery = encodeURIComponent(query.trim());

  const squad = await getJson<SquadDto>(
    `${API_BASE_URL}/api/squads?query=${encodedQuery}`,
  );

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
