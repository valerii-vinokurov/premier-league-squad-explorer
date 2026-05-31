import type { SquadDto } from "../models/squad";

interface SquadSummaryProps {
  squad: SquadDto;
}

export function SquadSummary({ squad }: SquadSummaryProps) {
  return (
    <section className="premium-card squad-summary p-4 p-lg-5 mb-4">
      <div className="d-flex flex-column flex-md-row align-items-md-center justify-content-between gap-4">
        <div className="d-flex align-items-center gap-3">
          {squad.team.logoUrl ? (
            <img
              src={squad.team.logoUrl}
              alt={`${squad.team.name} logo`}
              className="squad-summary-logo"
            />
          ) : (
            <div className="squad-summary-logo squad-summary-logo-placeholder">
              <i className="bi bi-shield-fill" aria-hidden="true" />
            </div>
          )}

          <div>
            <h2 className="h3 fw-bold mb-1">{squad.team.name}</h2>

            <p className="text-muted-soft mb-0">
              {squad.players.length} players loaded from {squad.metadata.source}
            </p>
          </div>
        </div>

        <div className="d-flex flex-wrap gap-2">
          <span className="meta-badge">
            <i className="bi bi-calendar3" aria-hidden="true" />
            Season {squad.metadata.season}/25
          </span>

          <span className="meta-badge">
            <i className="bi bi-database-check" aria-hidden="true" />
            {squad.metadata.cached ? "Cached" : "Live data"}
          </span>
        </div>
      </div>
    </section>
  );
}
