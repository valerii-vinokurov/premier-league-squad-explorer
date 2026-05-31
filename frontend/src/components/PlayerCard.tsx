import { useState } from "react";
import type { PlayerDto } from "../models/squad";

interface PlayerCardProps {
  player: PlayerDto;
}

export function PlayerCard({ player }: PlayerCardProps) {
  const [imageFailed, setImageFailed] = useState(false);

  const fullName = `${player.firstName} ${player.surname}`.trim();
  const displayName = fullName || player.displayName || "Unknown player";
  const showImage = player.profilePictureUrl && !imageFailed;

  return (
    <article className="premium-card player-card p-4 text-center">
      {showImage ? (
        <img
          src={player.profilePictureUrl ?? undefined}
          alt={displayName}
          className="player-photo mb-3"
          onError={() => setImageFailed(true)}
        />
      ) : (
        <div className="player-photo-placeholder mx-auto mb-3">
          <i className="bi bi-person-fill" aria-hidden="true" />
        </div>
      )}

      <h3 className="h5 fw-bold mb-1">{displayName}</h3>

      {player.displayName && player.displayName !== displayName && (
        <p className="text-muted-soft small mb-3">{player.displayName}</p>
      )}

      <div className="d-grid gap-2 text-start mt-3">
        <div className="player-meta-row">
          <span className="text-muted-soft small">
            <i className="bi bi-person-badge me-2" aria-hidden="true" />
            Position
          </span>

          <span className="fw-semibold">{player.position || "Unknown"}</span>
        </div>

        <div className="player-meta-row">
          <span className="text-muted-soft small">
            <i className="bi bi-calendar-event me-2" aria-hidden="true" />
            DOB
          </span>

          <span className="fw-semibold">{player.dateOfBirth || "Unknown"}</span>
        </div>
      </div>
    </article>
  );
}
