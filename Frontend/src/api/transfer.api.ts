import { get, post, del } from "./client";
import type { TransferCreate, TransferResponse } from "../types";

export const transferApi = {
  getAll: () => get<TransferResponse[]>("/transfer"),
  getById: (id: string) => get<TransferResponse>(`/transfer/${id}`),
  getByStatus: (status: string) => get<TransferResponse[]>(`/transfer/status/${status}`),
  create: (data: TransferCreate) => post<TransferResponse>("/transfer", data),
  remove: (id: string) => del(`/transfer/${id}`),
};
