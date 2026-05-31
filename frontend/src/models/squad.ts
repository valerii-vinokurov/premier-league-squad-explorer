export interface TeamDto {
  id: number;
  name: string;
  providerName?: string | null;
  logoUrl?: string | null;
}

export interface PlayerDto {
  id: number;
  firstName: string;
  surname: string;
  dateOfBirth?: string | null;
  position?: string | null;
  profilePictureUrl?: string | null;
  displayName: string;
}

export interface SquadMetadataDto {
  leagueId: number;
  season: number;
  source: string;
  cached: boolean;
}

export interface SquadDto {
  team: TeamDto;
  players: PlayerDto[];
  metadata: SquadMetadataDto;
}

export interface ErrorResponseDto {
  code: string;
  message: string;
}
