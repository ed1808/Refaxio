import { createCrudPage } from "./crud-helper";
import { productApi } from "../api/product.api";
import { categoryApi } from "../api/category.api";
import type { ProductResponse, ProductCreate, ProductUpdate, CategoryResponse } from "../types";
import { formatCurrency, formatDate } from "../utils/format";

export const productsPage = createCrudPage<ProductResponse, ProductCreate, ProductUpdate>({
  title: "Productos",
  entityName: "Producto",
  columns: [
    { key: "sku", label: "SKU" },
    { key: "productName", label: "Nombre" },
    { key: "brand", label: "Marca" },
    { key: "categoryName", label: "Categoría" },
    { key: "purchasePrice", label: "P. Compra", render: (i) => formatCurrency(i.purchasePrice) },
    { key: "salePrice", label: "P. Venta", render: (i) => formatCurrency(i.salePrice) },
    { key: "createdAt", label: "Creado", render: (i) => formatDate(i.createdAt) },
  ],
  getAll: productApi.getAll,
  create: productApi.create,
  update: (id, data) => productApi.update(id as string, data),
  remove: (id) => productApi.remove(id as string),
  getId: (i) => i.sku,
  loadLookups: async () => {
    const categories = await categoryApi.getAll();
    return { categories };
  },
  getCreateFields: (lookups) => {
    const cats = (lookups?.categories ?? []) as CategoryResponse[];
    return [
      { name: "sku", label: "SKU", required: true, maxLength: 20 },
      { name: "productName", label: "Nombre", required: true, maxLength: 255 },
      { name: "productDescription", label: "Descripción", type: "textarea", maxLength: 500 },
      { name: "purchasePrice", label: "Precio Compra", type: "number", required: true, step: "0.01", min: "0" },
      { name: "salePrice", label: "Precio Venta", type: "number", required: true, step: "0.01", min: "0" },
      { name: "brand", label: "Marca", required: true, maxLength: 100 },
      { name: "categoryId", label: "Categoría", type: "select", required: true, options: cats.map((c) => ({ value: c.id, label: c.categoryName })) },
    ];
  },
  getUpdateFields: (item, lookups) => {
    const cats = (lookups?.categories ?? []) as CategoryResponse[];
    return [
      { name: "productName", label: "Nombre", required: true, value: item.productName, maxLength: 255 },
      { name: "productDescription", label: "Descripción", type: "textarea", value: item.productDescription ?? "", maxLength: 500 },
      { name: "purchasePrice", label: "Precio Compra", type: "number", required: true, value: item.purchasePrice, step: "0.01", min: "0" },
      { name: "salePrice", label: "Precio Venta", type: "number", required: true, value: item.salePrice, step: "0.01", min: "0" },
      { name: "brand", label: "Marca", required: true, value: item.brand, maxLength: 100 },
      { name: "categoryId", label: "Categoría", type: "select", required: true, value: item.categoryId, options: cats.map((c) => ({ value: c.id, label: c.categoryName })) },
    ];
  },
  mapFormToCreate: (d) => ({
    sku: d.sku,
    productName: d.productName,
    productDescription: d.productDescription || undefined,
    purchasePrice: parseFloat(d.purchasePrice),
    salePrice: parseFloat(d.salePrice),
    brand: d.brand,
    categoryId: d.categoryId,
  }),
  mapFormToUpdate: (d) => ({
    productName: d.productName,
    productDescription: d.productDescription || undefined,
    purchasePrice: parseFloat(d.purchasePrice),
    salePrice: parseFloat(d.salePrice),
    brand: d.brand,
    categoryId: d.categoryId,
  }),
});
