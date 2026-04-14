import { createCrudPage } from "./crud-helper";
import { categoryApi } from "../api/category.api";
import type { CategoryResponse, CategoryCreate, CategoryUpdate } from "../types";
import { formatDate } from "../utils/format";

export const categoriesPage = createCrudPage<CategoryResponse, CategoryCreate, CategoryUpdate>({
  title: "Categorías",
  entityName: "Categoría",
  columns: [
    { key: "categoryName", label: "Nombre" },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: categoryApi.getAll,
  create: categoryApi.create,
  update: (id, data) => categoryApi.update(id as string, data),
  remove: (id) => categoryApi.remove(id as string),
  getId: (i) => i.id,
  getCreateFields: () => [
    { name: "categoryName", label: "Nombre", required: true, maxLength: 200 },
  ],
  getUpdateFields: (item) => [
    { name: "categoryName", label: "Nombre", required: true, value: item.categoryName, maxLength: 200 },
  ],
  mapFormToCreate: (d) => ({ categoryName: d.categoryName }),
  mapFormToUpdate: (d) => ({ categoryName: d.categoryName }),
});
