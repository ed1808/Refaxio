import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderTable, type Column } from "../components/table";
import { openModal, closeModal } from "../components/modal";
import { renderForm } from "../components/form-field";
import { showToast } from "../components/toast";
import { inventoryApi } from "../api/inventory.api";
import type { InventoryResponse } from "../types";
import { formatDateTime } from "../utils/format";

export async function inventoryPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  try {
    const data = await inventoryApi.getAll();

    const columns: Column<InventoryResponse>[] = [
      { key: "productSku", label: "SKU" },
      { key: "productName", label: "Producto" },
      { key: "storageName", label: "Almacén" },
      { key: "stock", label: "Stock", render: (i) => {
        const color = i.stock <= i.minStock ? "text-red-600 font-semibold" : "text-gray-700";
        return `<span class="${color}">${i.stock}</span>`;
      }},
      { key: "minStock", label: "Stock Mín." },
      { key: "location", label: "Ubicación" },
      { key: "updatedAt", label: "Actualizado", render: (i) => formatDateTime(i.updatedAt) },
    ];

    content.innerHTML = `
      <div class="mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Inventario</h1>
        <p class="text-gray-500 text-sm mt-1">Stock actual por producto y almacén</p>
      </div>
      <div class="bg-white rounded-xl border border-gray-200" id="table-container"></div>
    `;

    const tableContainer = document.getElementById("table-container")!;
    renderTable(tableContainer, columns, data, [
      {
        label: "Editar Stock",
        variant: "primary",
        onClick: (item) => {
          const form = renderForm(
            [
              { name: "stock", label: "Stock", type: "number", required: true, value: item.stock, min: "0" },
              { name: "minStock", label: "Stock Mínimo", type: "number", required: true, value: item.minStock, min: "0" },
              { name: "location", label: "Ubicación", value: item.location ?? "", maxLength: 255 },
            ],
            async (formData) => {
              try {
                await inventoryApi.update(item.productSku, item.storageId, {
                  stock: parseInt(formData.stock),
                  minStock: parseInt(formData.minStock),
                  location: formData.location || undefined,
                });
                closeModal();
                showToast("Inventario actualizado", "success");
                await inventoryPage();
              } catch (err) {
                showToast(err instanceof Error ? err.message : "Error al actualizar", "error");
              }
            },
          );
          openModal({ title: `Editar: ${item.productName} — ${item.storageName}`, content: form });
        },
      },
    ]);
  } catch (err) {
    content.innerHTML = `<p class="text-red-500">Error: ${err instanceof Error ? err.message : "Error"}</p>`;
  }
}
