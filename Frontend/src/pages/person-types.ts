import { createCrudPage } from "./crud-helper";
import { personTypeApi } from "../api/personType.api";
import type { PersonTypeResponse, PersonTypeCreate, PersonTypeUpdate } from "../types";
import { formatDate } from "../utils/format";

export const personTypesPage = createCrudPage<PersonTypeResponse, PersonTypeCreate, PersonTypeUpdate>({
  title: "Tipos de Persona",
  entityName: "Tipo de Persona",
  columns: [
    { key: "personTypeName", label: "Nombre" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: personTypeApi.getAll,
  create: personTypeApi.create,
  update: (id, data) => personTypeApi.update(id as string, data),
  remove: (id) => personTypeApi.remove(id as string),
  getId: (i) => i.id,
  getCreateFields: () => [
    { name: "personTypeName", label: "Nombre", required: true, maxLength: 150 },
  ],
  getUpdateFields: (item) => [
    { name: "personTypeName", label: "Nombre", required: true, value: item.personTypeName, maxLength: 150 },
  ],
  mapFormToCreate: (d) => ({ personTypeName: d.personTypeName }),
  mapFormToUpdate: (d) => ({ personTypeName: d.personTypeName }),
});
