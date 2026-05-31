export function AppHeader() {
  return (
    <section className="app-hero p-4 p-md-5 mb-4">
      <div className="app-hero-content row align-items-center g-4">
        <div className="col-12 col-lg-8">
          <span className="meta-badge bg-white bg-opacity-10 text-white mb-3">
            <i className="bi bi-trophy-fill" aria-hidden="true" />
            EPL 2024/25
          </span>

          <h1 className="display-5 fw-bold mb-3">
            Premier League Squad Explorer
          </h1>

          <p className="lead mb-0 text-white-50">
            Search for an English Premier League club by official name or
            nickname and view squad player details.
          </p>
        </div>

        <div className="col-12 col-lg-4">
          <div className="premium-card bg-white bg-opacity-10 border-0 p-4 text-white">
            <div className="d-flex align-items-center gap-3">
              <div className="display-6">
                <i className="bi bi-shield-check" aria-hidden="true" />
              </div>

              <div>
                <div className="fw-semibold">Full-stack POC</div>
                <div className="small text-white-50">
                  React → C# API → API-Football
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
