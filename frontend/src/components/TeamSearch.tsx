import { useState, type FormEvent } from "react";
import type { TeamDto } from "../models/squad";

interface TeamSearchProps {
  teams?: TeamDto[];
  isLoading: boolean;
  onSearch: (query: string) => void;
}

const exampleQueries = [
  "Arsenal",
  "Manchester United",
  "West Ham United",
  "The Hammers",
];

export function TeamSearch({
  teams = [],
  isLoading,
  onSearch,
}: TeamSearchProps) {
  const [query, setQuery] = useState("");

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    onSearch(query);
  }

  function handleExampleClick(example: string) {
    setQuery(example);
    onSearch(example);
  }

  return (
    <section className="premium-card search-card p-4 p-lg-5">
      <div className="row g-4 align-items-end">
        <div className="col-12 col-lg">
          <div className="d-flex align-items-center gap-2 mb-2">
            <span className="meta-badge">
              <i className="bi bi-search" aria-hidden="true" />
              Team search
            </span>
          </div>

          <h2 className="h4 fw-bold mb-2">Find a Premier League squad</h2>

          <p className="text-muted-soft mb-0">
            Search by official club name or supported nickname, for example West
            Ham United or The Hammers.
          </p>
        </div>

        <div className="col-12 col-lg-7">
          <form onSubmit={handleSubmit}>
            <label htmlFor="teamQuery" className="form-label fw-semibold">
              Club name or nickname
            </label>

            <div className="input-group input-group-lg">
              <span className="input-group-text bg-white">
                <i
                  className="bi bi-shield-fill-check text-primary"
                  aria-hidden="true"
                />
              </span>

              <input
                id="teamQuery"
                name="teamQuery"
                className="form-control"
                type="search"
                placeholder="Try West Ham United or The Hammers"
                autoComplete="off"
                list="team-suggestions"
                value={query}
                disabled={isLoading}
                onChange={(event) => setQuery(event.target.value)}
              />

              <button
                className="btn btn-primary px-4"
                type="submit"
                disabled={isLoading}
              >
                {isLoading ? (
                  <>
                    <span
                      className="spinner-border spinner-border-sm me-2"
                      aria-hidden="true"
                    />
                    Loading
                  </>
                ) : (
                  <>
                    <i
                      className="bi bi-arrow-right-circle me-2"
                      aria-hidden="true"
                    />
                    Search
                  </>
                )}
              </button>
            </div>

            <datalist id="team-suggestions">
              {teams.map((team) => (
                <option key={team.id} value={team.name} />
              ))}
            </datalist>

            <div className="d-flex flex-wrap gap-2 mt-3">
              {exampleQueries.map((example) => (
                <button
                  key={example}
                  type="button"
                  className="btn btn-sm btn-outline-secondary rounded-pill"
                  disabled={isLoading}
                  onClick={() => handleExampleClick(example)}
                >
                  {example}
                </button>
              ))}
            </div>
          </form>
        </div>
      </div>
    </section>
  );
}
