import { get, post, put, del } from "./client";
import type { RoleCreate, RoleUpdate, RoleResponse } from "../types";

export const roleApi = {
  getAll: () => get<RoleResponse[]>("/role"),
  getById: (id: string) => get<RoleResponse>(`/role/${id}`),
  create: (data: RoleCreate) => post<RoleResponse>("/role", data),
  update: (id: string, data: RoleUpdate) => put<RoleResponse>(`/role/${id}`, data),
  remove: (id: string) => del(`/role/${id}`),
};
