import { createCrudPage } from "./crud-helper";
import { providerApi } from "../api/provider.api";
import { documentIdTypeApi } from "../api/documentIdType.api";
import { personTypeApi } from "../api/personType.api";
import type { ProviderResponse, ProviderCreate, ProviderUpdate, DocumentIdTypeResponse, PersonTypeResponse } from "../types";
import { formatDate } from "../utils/format";

export const providersPage = createCrudPage<ProviderResponse, ProviderCreate, ProviderUpdate>({
  title: "Proveedores",
  entityName: "Proveedor",
  columns: [
    { key: "firstName", label: "Nombre", render: (i) => `${i.firstName} ${i.middleName ?? ""} ${i.firstSurname ?? ""} ${i.secondSurname ?? ""}`.trim() },
    { key: "documentIdNumber", label: "Documento" },
    { key: "documentIdName", label: "Tipo Doc." },
    { key: "email", label: "Email" },
    { key: "telephoneNumber", label: "Teléfono" },
    { key: "address", label: "Dirección" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: providerApi.getAll,
  create: providerApi.create,
  update: (id, data) => providerApi.update(id as string, data),
  remove: (id) => providerApi.remove(id as string),
  getId: (i) => i.id,
  loadLookups: async () => {
    const [docTypes, personTypes] = await Promise.all([
      documentIdTypeApi.getAll(),
      personTypeApi.getAll(),
    ]);
    return { docTypes, personTypes };
  },
  getCreateFields: (lookups) => {
    const docTypes = (lookups?.docTypes ?? []) as DocumentIdTypeResponse[];
    const personTypes = (lookups?.personTypes ?? []) as PersonTypeResponse[];
    return [
      { name: "firstName", label: "Primer Nombre", required: true, maxLength: 150 },
      { name: "middleName", label: "Segundo Nombre", maxLength: 150 },
      { name: "firstSurname", label: "Primer Apellido", maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "personTypeId", label: "Tipo Persona", type: "select", required: true, options: personTypes.map((p) => ({ value: p.id, label: p.personTypeName })) },
      { name: "email", label: "Email", type: "email", required: true, maxLength: 254 },
      { name: "telephoneNumber", label: "Teléfono", required: true, maxLength: 10 },
      { name: "address", label: "Dirección", required: true, maxLength: 255 },
    ];
  },
  getUpdateFields: (item, lookups) => {
    const docTypes = (lookups?.docTypes ?? []) as DocumentIdTypeResponse[];
    const personTypes = (lookups?.personTypes ?? []) as PersonTypeResponse[];
    return [
      { name: "firstName", label: "Primer Nombre", required: true, value: item.firstName, maxLength: 150 },
      { name: "middleName", label: "Segundo Nombre", value: item.middleName ?? "", maxLength: 150 },
      { name: "firstSurname", label: "Primer Apellido", value: item.firstSurname ?? "", maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", value: item.secondSurname ?? "", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, value: item.documentIdNumber, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, value: item.docTypeId, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "personTypeId", label: "Tipo Persona", type: "select", required: true, value: item.personTypeId, options: personTypes.map((p) => ({ value: p.id, label: p.personTypeName })) },
      { name: "email", label: "Email", type: "email", required: true, value: item.email, maxLength: 254 },
      { name: "telephoneNumber", label: "Teléfono", required: true, value: item.telephoneNumber, maxLength: 10 },
      { name: "address", label: "Dirección", required: true, value: item.address, maxLength: 255 },
    ];
  },
  mapFormToCreate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname || undefined,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    personTypeId: d.personTypeId,
    email: d.email,
    telephoneNumber: d.telephoneNumber,
    address: d.address,
  }),
  mapFormToUpdate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname || undefined,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    personTypeId: d.personTypeId,
    email: d.email,
    telephoneNumber: d.telephoneNumber,
    address: d.address,
  }),
});
