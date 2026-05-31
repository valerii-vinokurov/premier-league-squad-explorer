export function decodeHtml(value: string | null | undefined): string {
  if (!value) {
    return "";
  }

  const textarea = document.createElement("textarea");
  textarea.innerHTML = value;

  return textarea.value;
}

export function decodeHtmlOrNull(
  value: string | null | undefined,
): string | null {
  const decoded = decodeHtml(value).trim();

  return decoded || null;
}
