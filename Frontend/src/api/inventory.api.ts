import { get, put } from "./client";
import type { InventoryUpdate, InventoryResponse } from "../types";

export const inventoryApi = {
  getAll: () => get<InventoryResponse[]>("/inventory"),
  get: (sku: string, storageId: number) => get<InventoryResponse>(`/inventory/${sku}/${storageId}`),
  getByProduct: (sku: string) => get<InventoryResponse[]>(`/inventory/product/${sku}`),
  getByStorage: (storageId: number) => get<InventoryResponse[]>(`/inventory/storage/${storageId}`),
  update: (sku: string, storageId: number, data: InventoryUpdate) => put<InventoryResponse>(`/inventory/${sku}/${storageId}`, data),
};
