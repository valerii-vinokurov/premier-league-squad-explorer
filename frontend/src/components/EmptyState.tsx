export function EmptyState() {
  return (
    <div className="premium-card empty-state p-4 p-lg-5 text-center">
      <div className="display-5 text-primary mb-3">
        <i className="bi bi-search" aria-hidden="true" />
      </div>

      <h2 className="h4 fw-bold mb-2">Start with a team search</h2>

      <p className="text-muted-soft mb-0">
        Try Manchester United, Arsenal, West Ham United, or the bonus nickname
        The Hammers.
      </p>
    </div>
  );
}
