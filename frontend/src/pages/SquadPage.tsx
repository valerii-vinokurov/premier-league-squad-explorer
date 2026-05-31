import { useEffect, useState } from "react";
import { getTeams } from "../api/squadApi";
import { AppFooter } from "../components/AppFooter";
import { AppHeader } from "../components/AppHeader";
import { EmptyState } from "../components/EmptyState";
import { ErrorMessage } from "../components/ErrorMessage";
import { LoadingState } from "../components/LoadingState";
import { SquadGrid } from "../components/SquadGrid";
import { SquadSummary } from "../components/SquadSummary";
import { TeamSearch } from "../components/TeamSearch";
import { useSquadSearch } from "../hooks/useSquadSearch";
import type { TeamDto } from "../models/squad";

export function SquadPage() {
  const [teams, setTeams] = useState<TeamDto[]>([]);
  const [teamsError, setTeamsError] = useState<string | null>(null);

  const { squad, isLoading, error, hasSearched, searchSquad } =
    useSquadSearch();

  useEffect(() => {
    let isMounted = true;

    async function loadTeams() {
      try {
        const result = await getTeams();

        if (isMounted) {
          setTeams(result);
        }
      } catch {
        if (isMounted) {
          setTeamsError(
            "Team suggestions could not be loaded, but you can still search manually.",
          );
        }
      }
    }

    loadTeams();

    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <main className="app-shell">
      <div className="container-xxl app-container px-3 px-md-4 py-4 py-lg-5">
        <AppHeader />

        <TeamSearch
          teams={teams}
          isLoading={isLoading}
          onSearch={searchSquad}
        />

        {teamsError && (
          <div className="alert alert-warning mt-4 mb-0" role="alert">
            <i className="bi bi-exclamation-triangle me-2" aria-hidden="true" />
            {teamsError}
          </div>
        )}

        <section className="mt-4">
          {isLoading && <LoadingState />}

          {!isLoading && error && <ErrorMessage message={error} />}

          {!isLoading && !error && !squad && !hasSearched && <EmptyState />}

          {!isLoading && !error && squad && (
            <div className="fade-in-up">
              <SquadSummary squad={squad} />
              <SquadGrid players={squad.players} />
            </div>
          )}
        </section>

        <AppFooter />
      </div>
    </main>
  );
}
