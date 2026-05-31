# Premier League Squad Explorer

![CI](https://github.com/valerii-vinokurov/premier-league-squad-explorer/actions/workflows/ci.yml/badge.svg)

## CI/CD

The repository uses a lightweight GitHub Actions pipeline to validate the project on every push and pull request to `master`.

The pipeline performs the following checks:

- restores back-end dependencies;
- builds the ASP.NET Core back-end;
- runs the back-end test suite;
- installs front-end dependencies with `npm ci`;
- builds the React front-end with Vite.

Deployment is intentionally out of scope for this proof of concept. The pipeline focuses on restore, build, test, and front-end build validation to keep the submission simple and reliable.