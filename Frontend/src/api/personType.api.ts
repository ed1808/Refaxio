import { get, post, put, del } from "./client";
import type { PersonTypeCreate, PersonTypeUpdate, PersonTypeResponse } from "../types";

export const personTypeApi = {
  getAll: () => get<PersonTypeResponse[]>("/persontype"),
  getById: (id: string) => get<PersonTypeResponse>(`/persontype/${id}`),
  create: (data: PersonTypeCreate) => post<PersonTypeResponse>("/persontype", data),
  update: (id: string, data: PersonTypeUpdate) => put<PersonTypeResponse>(`/persontype/${id}`, data),
  remove: (id: string) => del(`/persontype/${id}`),
};
