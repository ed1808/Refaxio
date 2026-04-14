import { get, post, put, del } from "./client";
import type { StorageCreate, StorageUpdate, StorageResponse } from "../types";

export const storageApi = {
  getAll: () => get<StorageResponse[]>("/storage"),
  getById: (id: number) => get<StorageResponse>(`/storage/${id}`),
  create: (data: StorageCreate) => post<StorageResponse>("/storage", data),
  update: (id: number, data: StorageUpdate) => put<StorageResponse>(`/storage/${id}`, data),
  remove: (id: number) => del(`/storage/${id}`),
};
