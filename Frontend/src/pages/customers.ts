import { createCrudPage } from "./crud-helper";
import { customerApi } from "../api/customer.api";
import { documentIdTypeApi } from "../api/documentIdType.api";
import { personTypeApi } from "../api/personType.api";
import type { CustomerResponse, CustomerCreate, CustomerUpdate, DocumentIdTypeResponse, PersonTypeResponse } from "../types";
import { formatDate } from "../utils/format";

export const customersPage = createCrudPage<CustomerResponse, CustomerCreate, CustomerUpdate>({
  title: "Clientes",
  entityName: "Cliente",
  columns: [
    { key: "firstName", label: "Nombre", render: (i) => `${i.firstName} ${i.middleName ?? ""} ${i.firstSurname} ${i.secondSurname ?? ""}`.trim() },
    { key: "documentIdNumber", label: "Documento" },
    { key: "documentIdName", label: "Tipo Doc." },
    { key: "personTypeName", label: "Tipo Persona" },
    { key: "email", label: "Email" },
    { key: "telephoneNumber", label: "Teléfono" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: customerApi.getAll,
  create: customerApi.create,
  update: (id, data) => customerApi.update(id as string, data),
  remove: (id) => customerApi.remove(id as string),
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
      { name: "firstSurname", label: "Primer Apellido", required: true, maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "personTypeId", label: "Tipo Persona", type: "select", required: true, options: personTypes.map((p) => ({ value: p.id, label: p.personTypeName })) },
      { name: "email", label: "Email", type: "email", maxLength: 254 },
      { name: "telephoneNumber", label: "Teléfono", maxLength: 10 },
    ];
  },
  getUpdateFields: (item, lookups) => {
    const docTypes = (lookups?.docTypes ?? []) as DocumentIdTypeResponse[];
    const personTypes = (lookups?.personTypes ?? []) as PersonTypeResponse[];
    return [
      { name: "firstName", label: "Primer Nombre", required: true, value: item.firstName, maxLength: 150 },
      { name: "middleName", label: "Segundo Nombre", value: item.middleName ?? "", maxLength: 150 },
      { name: "firstSurname", label: "Primer Apellido", required: true, value: item.firstSurname, maxLength: 150 },
      { name: "secondSurname", label: "Segundo Apellido", value: item.secondSurname ?? "", maxLength: 150 },
      { name: "documentIdNumber", label: "Nro. Documento", required: true, value: item.documentIdNumber, maxLength: 50 },
      { name: "docTypeId", label: "Tipo Documento", type: "select", required: true, value: item.docTypeId, options: docTypes.map((d) => ({ value: d.id, label: d.documentIdName })) },
      { name: "personTypeId", label: "Tipo Persona", type: "select", required: true, value: item.personTypeId, options: personTypes.map((p) => ({ value: p.id, label: p.personTypeName })) },
      { name: "email", label: "Email", type: "email", value: item.email ?? "", maxLength: 254 },
      { name: "telephoneNumber", label: "Teléfono", value: item.telephoneNumber ?? "", maxLength: 10 },
    ];
  },
  mapFormToCreate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    personTypeId: d.personTypeId,
    email: d.email || undefined,
    telephoneNumber: d.telephoneNumber || undefined,
  }),
  mapFormToUpdate: (d) => ({
    firstName: d.firstName,
    middleName: d.middleName || undefined,
    firstSurname: d.firstSurname,
    secondSurname: d.secondSurname || undefined,
    documentIdNumber: d.documentIdNumber,
    docTypeId: d.docTypeId,
    personTypeId: d.personTypeId,
    email: d.email || undefined,
    telephoneNumber: d.telephoneNumber || undefined,
  }),
});
