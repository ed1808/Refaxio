import { get, post, put, del } from "./client";
import type { PurchaseCreate, PurchaseUpdate, PurchaseResponse } from "../types";

export const purchaseApi = {
  getAll: () => get<PurchaseResponse[]>("/purchase"),
  getById: (id: string) => get<PurchaseResponse>(`/purchase/${id}`),
  getByProvider: (providerId: string) => get<PurchaseResponse[]>(`/purchase/provider/${providerId}`),
  getByStatus: (status: string) => get<PurchaseResponse[]>(`/purchase/status/${status}`),
  create: (data: PurchaseCreate) => post<PurchaseResponse>("/purchase", data),
  update: (id: string, data: PurchaseUpdate) => put<PurchaseResponse>(`/purchase/${id}`, data),
  remove: (id: string) => del(`/purchase/${id}`),
};
