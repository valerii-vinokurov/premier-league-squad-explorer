import { useCallback, useState } from "react";
import { ApiError, getSquad } from "../api/squadApi";
import type { SquadDto } from "../models/squad";

interface UseSquadSearchResult {
  squad: SquadDto | null;
  isLoading: boolean;
  error: string | null;
  hasSearched: boolean;
  searchSquad: (query: string) => Promise<void>;
  clear: () => void;
}

export function useSquadSearch(): UseSquadSearchResult {
  const [squad, setSquad] = useState<SquadDto | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [hasSearched, setHasSearched] = useState(false);

  const searchSquad = useCallback(async (query: string) => {
    const trimmedQuery = query.trim();

    if (!trimmedQuery) {
      setSquad(null);
      setError(
        "Please enter a Premier League team name or supported nickname.",
      );
      setHasSearched(true);
      return;
    }

    setIsLoading(true);
    setError(null);
    setHasSearched(true);

    try {
      const result = await getSquad(trimmedQuery);
      setSquad(result);
    } catch (exception) {
      setSquad(null);

      if (exception instanceof ApiError) {
        setError(exception.message);
        return;
      }

      setError("Unable to load squad data. Please try again later.");
    } finally {
      setIsLoading(false);
    }
  }, []);

  const clear = useCallback(() => {
    setSquad(null);
    setError(null);
    setIsLoading(false);
    setHasSearched(false);
  }, []);

  return {
    squad,
    isLoading,
    error,
    hasSearched,
    searchSquad,
    clear,
  };
}
