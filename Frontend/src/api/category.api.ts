import { get, post, put, del } from "./client";
import type { CategoryCreate, CategoryUpdate, CategoryResponse } from "../types";

export const categoryApi = {
  getAll: () => get<CategoryResponse[]>("/category"),
  getById: (id: string) => get<CategoryResponse>(`/category/${id}`),
  create: (data: CategoryCreate) => post<CategoryResponse>("/category", data),
  update: (id: string, data: CategoryUpdate) => put<CategoryResponse>(`/category/${id}`, data),
  remove: (id: string) => del(`/category/${id}`),
};
