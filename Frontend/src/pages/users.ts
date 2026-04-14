import { createCrudPage } from "./crud-helper";
import { userApi } from "../api/user.api";
import { documentIdTypeApi } from "../api/documentIdType.api";
import { roleApi } from "../api/role.api";
import type { UserResponse, UserCreate, UserUpdate, DocumentIdTypeResponse, RoleResponse } from "../types";
import { formatDate } from "../utils/format";

export const usersPage = createCrudPage<UserResponse, UserCreate, UserUpdate>({
  title: "Usuarios",
  entityName: "Usuario",
  columns: [
    { key: "username", label: "Usuario" },
    { key: "firstName", label: "Nombre", render: (i) => `${i.firstName} ${i.middleName ?? ""} ${i.firstSurname} ${i.secondSurname ?? ""}`.trim() },
    { key: "documentIdNumber", label: "Documento" },
    { key: "roleName", label: "Rol" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: userApi.getAll,
  create: userApi.create,
  update: (id, data) => userApi.update(id as string, data),
  remove: (id) => userApi.remove(id as string),
  getId: (i) => i.id,
  loadLookups: async () => {
    const [docTypes, roles] = await Promise.all([
      documentIdTypeApi.getAll(),
      roleApi.getAll(),
    ]);
    return { docTypes, roles };
  },
  getCreateFields: (lookups) => {
    const docTypes = (lookups?.docTypes ?? []) as DocumentIdTypeResponse[];
    const roles = (lookups?.roles ?? []) as RoleResponse[];
    return [
      { name: "firstName", label: "Primer Nombre", required: true, maxLength: 150 },
      { name: "middleName", label: "Segundo Nombre", maxLength: 150 },
      { name: "firstSurname", label: "Primer Apellido", required: true, maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "username", label: "Usuario", required: true, maxLength: 100 },
      { name: "password", label: "Contraseña", type: "password", required: true },
      { name: "roleId", label: "Rol", type: "select", required: true, options: roles.map((r) => ({ value: r.id, label: r.roleName })) },
    ];
  },
  getUpdateFields: (item, lookups) => {
    const docTypes = (lookups?.docTypes ?? []) as DocumentIdTypeResponse[];
    const roles = (lookups?.roles ?? []) as RoleResponse[];
    return [
      { name: "firstName", label: "Primer Nombre", required: true, value: item.firstName, maxLength: 150 },
      { name: "middleName", label: "Segundo Nombre", value: item.middleName ?? "", maxLength: 150 },
      { name: "firstSurname", label: "Primer Apellido", required: true, value: item.firstSurname, maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", value: item.secondSurname ?? "", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, value: item.documentIdNumber, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, value: item.docTypeId, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "username", label: "Usuario", required: true, value: item.username, maxLength: 100 },
      { name: "password", label: "Contraseña", type: "password", required: true },
      { name: "roleId", label: "Rol", type: "select", required: true, value: item.roleId, options: roles.map((r) => ({ value: r.id, label: r.roleName })) },
    ];
  },
  mapFormToCreate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    username: d.username,
    password: d.password,
    roleId: d.roleId,
  }),
  mapFormToUpdate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    username: d.username,
    password: d.password,
    roleId: d.roleId,
  }),
});
