import { get, post, put, del } from "./client";
import type { CustomerCreate, CustomerUpdate, CustomerResponse } from "../types";

export const customerApi = {
  getAll: () => get<CustomerResponse[]>("/customer"),
  getById: (id: string) => get<CustomerResponse>(`/customer/${id}`),
  create: (data: CustomerCreate) => post<CustomerResponse>("/customer", data),
  update: (id: string, data: CustomerUpdate) => put<CustomerResponse>(`/customer/${id}`, data),
  remove: (id: string) => del(`/customer/${id}`),
};
