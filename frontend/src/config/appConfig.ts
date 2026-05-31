function getRequiredEnvValue(name: string, value: string | undefined): string {
  const trimmedValue = value?.trim();

  if (!trimmedValue) {
    throw new Error(`${name} is not configured.`);
  }

  return trimmedValue.replace(/\/+$/, "");
}

function validateUrl(name: string, value: string): string {
  try {
    return new URL(value).toString().replace(/\/+$/, "");
  } catch {
    throw new Error(`${name} must be a valid URL.`);
  }
}

const apiBaseUrl = validateUrl(
  "VITE_API_BASE_URL",
  getRequiredEnvValue("VITE_API_BASE_URL", import.meta.env.VITE_API_BASE_URL),
);

export const appConfig = {
  apiBaseUrl,
} as const;
