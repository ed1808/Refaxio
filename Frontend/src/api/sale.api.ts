import { get, post, put, del } from "./client";
import type { SaleCreate, SaleUpdate, SaleResponse } from "../types";

export const saleApi = {
  getAll: () => get<SaleResponse[]>("/sale"),
  getById: (id: string) => get<SaleResponse>(`/sale/${id}`),
  getByCustomer: (customerId: string) => get<SaleResponse[]>(`/sale/customer/${customerId}`),
  getByStatus: (status: string) => get<SaleResponse[]>(`/sale/status/${status}`),
  create: (data: SaleCreate) => post<SaleResponse>("/sale", data),
  update: (id: string, data: SaleUpdate) => put<SaleResponse>(`/sale/${id}`, data),
  remove: (id: string) => del(`/sale/${id}`),
};
