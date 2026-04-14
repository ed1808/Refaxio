import { createCrudPage } from "./crud-helper";
import { storageApi } from "../api/storage.api";
import type { StorageResponse, StorageCreate, StorageUpdate } from "../types";
import { formatDate } from "../utils/format";

export const storagesPage = createCrudPage<StorageResponse, StorageCreate, StorageUpdate>({
  title: "Almacenes",
  entityName: "Almacén",
  columns: [
    { key: "storageName", label: "Nombre" },
    { key: "address", label: "Dirección" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: storageApi.getAll,
  create: storageApi.create,
  update: (id, data) => storageApi.update(id as number, data),
  remove: (id) => storageApi.remove(id as number),
  getId: (i) => i.id,
  getCreateFields: () => [
    { name: "storageName", label: "Nombre", required: true, maxLength: 150 },
    { name: "address", label: "Dirección", maxLength: 255 },
  ],
  getUpdateFields: (item) => [
    { name: "storageName", label: "Nombre", required: true, value: item.storageName, maxLength: 150 },
    { name: "address", label: "Dirección", value: item.address ?? "", maxLength: 255 },
  ],
  mapFormToCreate: (d) => ({ storageName: d.storageName, address: d.address || undefined }),
  mapFormToUpdate: (d) => ({ storageName: d.storageName, address: d.address || undefined }),
});
