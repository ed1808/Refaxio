import { createCrudPage } from "./crud-helper";
import { roleApi } from "../api/role.api";
import type { RoleResponse, RoleCreate, RoleUpdate } from "../types";
import { formatDate } from "../utils/format";

export const rolesPage = createCrudPage<RoleResponse, RoleCreate, RoleUpdate>({
  title: "Roles",
  entityName: "Rol",
  columns: [
    { key: "roleName", label: "Nombre" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: roleApi.getAll,
  create: roleApi.create,
  update: (id, data) => roleApi.update(id as string, data),
  remove: (id) => roleApi.remove(id as string),
  getId: (i) => i.id,
  getCreateFields: () => [
    { name: "roleName", label: "Nombre del rol", required: true, maxLength: 100 },
  ],
  getUpdateFields: (item) => [
    { name: "roleName", label: "Nombre del rol", required: true, value: item.roleName, maxLength: 100 },
  ],
  mapFormToCreate: (d) => ({ roleName: d.roleName }),
  mapFormToUpdate: (d) => ({ roleName: d.roleName }),
});
