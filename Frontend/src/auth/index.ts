const TOKEN_KEY = "refaxio_token";

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token);
}

export function removeToken(): void {
  localStorage.removeItem(TOKEN_KEY);
}

interface JwtPayload {
  sub: string;
  unique_name: string;
  role: string;
  exp: number;
  [key: string]: unknown;
}

export function decodeJwt(): JwtPayload | null {
  const token = getToken();
  if (!token) return null;
  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(atob(base64));
    return JSON.parse(json) as JwtPayload;
  } catch {
    return null;
  }
}

export function getUserRole(): string {
  const payload = decodeJwt();
  return payload?.role ?? "";
}

export function getUsername(): string {
  const payload = decodeJwt();
  return payload?.unique_name ?? "";
}

export function getUserId(): string {
  const payload = decodeJwt();
  return payload?.sub ?? "";
}

export function isAuthenticated(): boolean {
  const payload = decodeJwt();
  if (!payload) return false;
  return payload.exp * 1000 > Date.now();
}
