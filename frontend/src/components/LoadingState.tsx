export function LoadingState() {
  return (
    <div className="premium-card p-5 text-center">
      <div className="spinner-border text-primary mb-3" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>

      <h2 className="h5 fw-bold mb-2">Loading squad data...</h2>

      <p className="text-muted-soft mb-0">
        We are resolving the club and loading player information.
      </p>
    </div>
  );
}
