import { get, post, put, del } from "./client";
import type { DocumentIdTypeCreate, DocumentIdTypeUpdate, DocumentIdTypeResponse } from "../types";

export const documentIdTypeApi = {
  getAll: () => get<DocumentIdTypeResponse[]>("/documentidtype"),
  getById: (id: string) => get<DocumentIdTypeResponse>(`/documentidtype/${id}`),
  create: (data: DocumentIdTypeCreate) => post<DocumentIdTypeResponse>("/documentidtype", data),
  update: (id: string, data: DocumentIdTypeUpdate) => put<DocumentIdTypeResponse>(`/documentidtype/${id}`, data),
  remove: (id: string) => del(`/documentidtype/${id}`),
};
