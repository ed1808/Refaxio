import { get, post, put, del } from "./client";
import type { UserCreate, UserUpdate, UserResponse } from "../types";

export const userApi = {
  getAll: () => get<UserResponse[]>("/user"),
  getById: (id: string) => get<UserResponse>(`/user/${id}`),
  getByUsername: (username: string) => get<UserResponse>(`/user/username/${username}`),
  create: (data: UserCreate) => post<UserResponse>("/user", data),
  update: (id: string, data: UserUpdate) => put<UserResponse>(`/user/${id}`, data),
  remove: (id: string) => del(`/user/${id}`),
};
