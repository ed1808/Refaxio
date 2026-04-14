import { getToken, removeToken } from "../auth";

const BASE_URL = "/api";

class ApiError extends Error {
    status: number;
  constructor(
    status: number,
    message: string,
  ) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}

async function request<T>(
  method: string,
  path: string,
  body?: unknown,
): Promise<T> {
  const headers: Record<string, string> = {};
  const token = getToken();
  if (token) headers["Authorization"] = `Bearer ${token}`;
  if (body) headers["Content-Type"] = "application/json";

  const res = await fetch(`${BASE_URL}${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (res.status === 401) {
    removeToken();
    location.hash = "#/login";
    throw new ApiError(401, "Sesión expirada");
  }

  if (res.status === 204) return undefined as T;

  if (!res.ok) {
    let msg = `Error ${res.status}`;
    try {
      const err = await res.json();
      msg = err.message || err.title || JSON.stringify(err);
    } catch {
      /* empty */
    }
    throw new ApiError(res.status, msg);
  }

  return res.json() as Promise<T>;
}

export function get<T>(path: string): Promise<T> {
  return request<T>("GET", path);
}

export function post<T>(path: string, body: unknown): Promise<T> {
  return request<T>("POST", path, body);
}

export function put<T>(path: string, body: unknown): Promise<T> {
  return request<T>("PUT", path, body);
}

export function del(path: string): Promise<void> {
  return request<void>("DELETE", path);
}

export { ApiError };
