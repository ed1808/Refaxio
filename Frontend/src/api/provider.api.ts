import { get, post, put, del } from "./client";
import type { ProviderCreate, ProviderUpdate, ProviderResponse } from "../types";

export const providerApi = {
  getAll: () => get<ProviderResponse[]>("/provider"),
  getById: (id: string) => get<ProviderResponse>(`/provider/${id}`),
  create: (data: ProviderCreate) => post<ProviderResponse>("/provider", data),
  update: (id: string, data: ProviderUpdate) => put<ProviderResponse>(`/provider/${id}`, data),
  remove: (id: string) => del(`/provider/${id}`),
};
