import type { ErrorResponseDto, SquadDto, TeamDto } from "../models/squad";

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
  return getJson<TeamDto[]>(`${API_BASE_URL}/api/teams`);
}

export async function getSquad(query: string): Promise<SquadDto> {
  const encodedQuery = encodeURIComponent(query.trim());

  return getJson<SquadDto>(`${API_BASE_URL}/api/squads?query=${encodedQuery}`);
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
