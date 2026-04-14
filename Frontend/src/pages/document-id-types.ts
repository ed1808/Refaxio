import { createCrudPage } from "./crud-helper";
import { documentIdTypeApi } from "../api/documentIdType.api";
import type { DocumentIdTypeResponse, DocumentIdTypeCreate, DocumentIdTypeUpdate } from "../types";
import { formatDate } from "../utils/format";

export const documentIdTypesPage = createCrudPage<DocumentIdTypeResponse, DocumentIdTypeCreate, DocumentIdTypeUpdate>({
  title: "Tipos de Documento",
  entityName: "Tipo de Documento",
  columns: [
    { key: "documentIdName", label: "Nombre" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: documentIdTypeApi.getAll,
  create: documentIdTypeApi.create,
  update: (id, data) => documentIdTypeApi.update(id as string, data),
  remove: (id) => documentIdTypeApi.remove(id as string),
  getId: (i) => i.id,
  getCreateFields: () => [
    { name: "documentIdName", label: "Nombre", required: true, maxLength: 150 },
  ],
  getUpdateFields: (item) => [
    { name: "documentIdName", label: "Nombre", required: true, value: item.documentIdName, maxLength: 150 },
  ],
  mapFormToCreate: (d) => ({ documentIdName: d.documentIdName }),
  mapFormToUpdate: (d) => ({ documentIdName: d.documentIdName }),
});
