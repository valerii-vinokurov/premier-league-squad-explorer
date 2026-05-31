import type { PlayerDto } from "../models/squad";
import { PlayerCard } from "./PlayerCard";

interface SquadGridProps {
  players: PlayerDto[];
}

export function SquadGrid({ players }: SquadGridProps) {
  return (
    <div className="row g-4">
      {players.map((player) => (
        <div key={player.id} className="col-12 col-sm-6 col-lg-4 col-xl-3">
          <PlayerCard player={player} />
        </div>
      ))}
    </div>
  );
}
