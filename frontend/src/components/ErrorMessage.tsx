interface ErrorMessageProps {
  message: string;
}

export function ErrorMessage({ message }: ErrorMessageProps) {
  return (
    <div className="alert alert-danger premium-card p-4 border-0" role="alert">
      <div className="d-flex gap-3">
        <i className="bi bi-x-circle-fill fs-4" aria-hidden="true" />

        <div>
          <h2 className="h5 fw-bold mb-1">Unable to load squad</h2>
          <p className="mb-0">{message}</p>
        </div>
      </div>
    </div>
  );
}
