import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderTable, type Column } from "../components/table";
import { inventoryMovementApi } from "../api/inventoryMovement.api";
import type { InventoryMovementResponse } from "../types";
import { formatDateTime } from "../utils/format";

export async function inventoryMovementsPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  try {
    const data = await inventoryMovementApi.getAll();

    const typeBadge = (type: string) => {
      const colors: Record<string, string> = {
        PURCHASE: "bg-blue-100 text-blue-700",
        SALE: "bg-green-100 text-green-700",
        TRANSFER_IN: "bg-purple-100 text-purple-700",
        TRANSFER_OUT: "bg-orange-100 text-orange-700",
      };
      const color = colors[type] ?? "bg-gray-100 text-gray-700";
      return `<span class="px-2 py-0.5 rounded-full text-xs font-medium ${color}">${type}</span>`;
    };

    const columns: Column<InventoryMovementResponse>[] = [
      { key: "productSku", label: "SKU" },
      { key: "productName", label: "Producto" },
      { key: "storageName", label: "Almacén" },
      { key: "movementType", label: "Tipo", render: (i) => typeBadge(i.movementType) },
      { key: "quantity", label: "Cantidad" },
      { key: "balanceAfter", label: "Saldo" },
      { key: "username", label: "Usuario" },
      { key: "createdAt", label: "Fecha", render: (i) => formatDateTime(i.createdAt) },
    ];

    content.innerHTML = `
      <div class="mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Movimientos de Inventario</h1>
        <p class="text-gray-500 text-sm mt-1">Historial de movimientos de stock</p>
      </div>
      <div class="bg-white rounded-xl border border-gray-200" id="table-container"></div>
    `;

    renderTable(document.getElementById("table-container")!, columns, data);
  } catch (err) {
    content.innerHTML = `<p class="text-red-500">Error: ${err instanceof Error ? err.message : "Error"}</p>`;
  }
}
