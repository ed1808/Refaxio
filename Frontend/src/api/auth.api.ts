import { post } from "./client";
import type { LoginRequest, AuthResponse } from "../types";

export function login(data: LoginRequest): Promise<AuthResponse> {
  return post<AuthResponse>("/auth/login", data);
}
