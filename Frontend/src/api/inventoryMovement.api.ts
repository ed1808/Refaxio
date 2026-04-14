import { get } from "./client";
import type { InventoryMovementResponse } from "../types";

export const inventoryMovementApi = {
  getAll: () => get<InventoryMovementResponse[]>("/inventorymovement"),
  getById: (id: string) => get<InventoryMovementResponse>(`/inventorymovement/${id}`),
  getByProduct: (sku: string) => get<InventoryMovementResponse[]>(`/inventorymovement/product/${sku}`),
  getByStorage: (storageId: number) => get<InventoryMovementResponse[]>(`/inventorymovement/storage/${storageId}`),
};
