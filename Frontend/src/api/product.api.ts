import { get, post, put, del } from "./client";
import type { ProductCreate, ProductUpdate, ProductResponse } from "../types";

export const productApi = {
  getAll: () => get<ProductResponse[]>("/product"),
  getBySku: (sku: string) => get<ProductResponse>(`/product/${sku}`),
  create: (data: ProductCreate) => post<ProductResponse>("/product", data),
  update: (sku: string, data: ProductUpdate) => put<ProductResponse>(`/product/${sku}`, data),
  remove: (sku: string) => del(`/product/${sku}`),
};
