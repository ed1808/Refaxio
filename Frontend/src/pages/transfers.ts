import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderTable, type Column } from "../components/table";
import { openModal, closeModal } from "../components/modal";
import { confirmDialog } from "../components/confirm-dialog";
import { showToast } from "../components/toast";
import { transferApi } from "../api/transfer.api";
import { productApi } from "../api/product.api";
import { storageApi } from "../api/storage.api";
import { getUserId } from "../auth";
import type { TransferResponse, TransferDetailCreate, ProductResponse, StorageResponse } from "../types";
import { formatDateTime } from "../utils/format";

function statusBadge(status: string | undefined): string {
  const s = status ?? "PENDING";
  const colors: Record<string, string> = {
    COMPLETED: "bg-green-100 text-green-700",
    PENDING: "bg-yellow-100 text-yellow-700",
    CANCELED: "bg-red-100 text-red-700",
  };
  const color = colors[s] ?? "bg-gray-100 text-gray-700";
  return `<span class="px-2 py-0.5 rounded-full text-xs font-medium ${color}">${s}</span>`;
}

function buildCreateForm(
  products: ProductResponse[],
  storages: StorageResponse[],
  onSubmit: (data: { originStorageId: number; destinationStorageId: number; details: TransferDetailCreate[] }) => Promise<void>,
): HTMLElement {
  const wrapper = document.createElement("div");
  wrapper.innerHTML = `
    <form id="transfer-form" class="space-y-4">
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Almacén Origen <span class="text-red-500">*</span></label>
        <select name="originStorageId" required class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500">
          <option value="">Seleccionar...</option>
          ${storages.map((s) => `<option value="${s.id}">${s.storageName}</option>`).join("")}
        </select>
      </div>
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Almacén Destino <span class="text-red-500">*</span></label>
        <select name="destinationStorageId" required class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500">
          <option value="">Seleccionar...</option>
          ${storages.map((s) => `<option value="${s.id}">${s.storageName}</option>`).join("")}
        </select>
      </div>
      <div class="border-t pt-4">
        <div class="flex items-center justify-between mb-2">
          <h3 class="text-sm font-semibold text-gray-900">Detalle</h3>
          <button type="button" id="add-line" class="text-xs text-indigo-600 hover:text-indigo-800 font-medium">+ Agregar línea</button>
        </div>
        <div id="detail-lines" class="space-y-3"></div>
      </div>
      <button type="submit" class="w-full px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 mt-2">Guardar Transferencia</button>
    </form>
  `;

  const linesContainer = wrapper.querySelector("#detail-lines")!;
  let lineCount = 0;

  function addLine(): void {
    const idx = lineCount++;
    const line = document.createElement("div");
    line.className = "grid grid-cols-3 gap-2 items-end bg-gray-50 p-3 rounded-lg";
    line.dataset.lineIdx = String(idx);
    line.innerHTML = `
      <div class="col-span-1">
        <label class="text-xs text-gray-500">Producto</label>
        <select name="detail_product_${idx}" required class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs">
          <option value="">Seleccionar</option>
          ${products.map((p) => `<option value="${p.sku}">${p.sku} — ${p.productName}</option>`).join("")}
        </select>
      </div>
      <div>
        <label class="text-xs text-gray-500">Cantidad</label>
        <input name="detail_qty_${idx}" type="number" min="1" required value="1" class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs" />
      </div>
      <div class="flex items-end">
        <button type="button" class="remove-line text-red-500 hover:text-red-700 pb-1.5 text-sm">&times;</button>
      </div>
    `;

    line.querySelector(".remove-line")!.addEventListener("click", () => line.remove());
    linesContainer.appendChild(line);
  }

  wrapper.querySelector("#add-line")!.addEventListener("click", addLine);
  addLine();

  const form = wrapper.querySelector("#transfer-form") as HTMLFormElement;
  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const fd = new FormData(form);

    const origin = parseInt(fd.get("originStorageId") as string);
    const destination = parseInt(fd.get("destinationStorageId") as string);
    if (origin === destination) {
      showToast("El almacén origen y destino no pueden ser iguales", "error");
      return;
    }

    const details: TransferDetailCreate[] = [];
    const lines = linesContainer.querySelectorAll("[data-line-idx]");
    for (const line of lines) {
      const i = line.getAttribute("data-line-idx")!;
      details.push({
        productSku: fd.get(`detail_product_${i}`) as string,
        quantity: parseInt(fd.get(`detail_qty_${i}`) as string),
      });
    }
    if (details.length === 0) {
      showToast("Agrega al menos una línea de detalle", "error");
      return;
    }
    await onSubmit({ originStorageId: origin, destinationStorageId: destination, details });
  });

  return wrapper;
}

function buildDetailView(transfer: TransferResponse): HTMLElement {
  const el = document.createElement("div");
  el.innerHTML = `
    <div class="space-y-3 text-sm">
      <div class="grid grid-cols-2 gap-4 mb-4">
        <div><span class="text-gray-500">Origen:</span> <strong>${transfer.originStorageName}</strong></div>
        <div><span class="text-gray-500">Destino:</span> <strong>${transfer.destinationStorageName}</strong></div>
        <div><span class="text-gray-500">Usuario:</span> <strong>${transfer.username}</strong></div>
        <div><span class="text-gray-500">Estado:</span> ${statusBadge(transfer.status)}</div>
        <div class="col-span-2"><span class="text-gray-500">Fecha:</span> ${formatDateTime(transfer.createdAt)}</div>
      </div>
      <table class="w-full text-xs">
        <thead class="bg-gray-50"><tr>
          <th class="px-3 py-2 text-left">SKU</th>
          <th class="px-3 py-2 text-left">Producto</th>
          <th class="px-3 py-2 text-right">Cantidad</th>
        </tr></thead>
        <tbody>
          ${transfer.details.map((d) => `
            <tr class="border-t">
              <td class="px-3 py-2">${d.productSku}</td>
              <td class="px-3 py-2">${d.productName}</td>
              <td class="px-3 py-2 text-right">${d.quantity}</td>
            </tr>`).join("")}
        </tbody>
      </table>
    </div>
  `;
  return el;
}

export async function transfersPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  try {
    const data = await transferApi.getAll();

    const columns: Column<TransferResponse>[] = [
      { key: "originStorageName", label: "Origen" },
      { key: "destinationStorageName", label: "Destino" },
      { key: "username", label: "Usuario" },
      { key: "status", label: "Estado", render: (i) => statusBadge(i.status) },
      { key: "createdAt", label: "Fecha", render: (i) => formatDateTime(i.createdAt) },
    ];

    content.innerHTML = `
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Transferencias</h1>
        <button id="create-btn" class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700">+ Nueva Transferencia</button>
      </div>
      <div class="bg-white rounded-xl border border-gray-200" id="table-container"></div>
    `;

    renderTable(document.getElementById("table-container")!, columns, data, [
      {
        label: "Ver",
        variant: "primary",
        onClick: async (item) => {
          const transfer = await transferApi.getById(item.id);
          openModal({ title: "Detalle de Transferencia", content: buildDetailView(transfer) });
        },
      },
      {
        label: "Eliminar",
        variant: "danger",
        onClick: (item) => {
          confirmDialog("¿Eliminar esta transferencia?", async () => {
            try {
              await transferApi.remove(item.id);
              showToast("Transferencia eliminada", "success");
              await transfersPage();
            } catch (err) {
              showToast(err instanceof Error ? err.message : "Error", "error");
            }
          });
        },
      },
    ]);

    document.getElementById("create-btn")!.addEventListener("click", async () => {
      const [products, storages] = await Promise.all([
        productApi.getAll(),
        storageApi.getAll(),
      ]);
      const formEl = buildCreateForm(products, storages, async (formData) => {
        try {
          await transferApi.create({
            ...formData,
            userId: getUserId(),
            status: "COMPLETED",
          });
          closeModal();
          showToast("Transferencia creada", "success");
          await transfersPage();
        } catch (err) {
          showToast(err instanceof Error ? err.message : "Error", "error");
        }
      });
      openModal({ title: "Nueva Transferencia", content: formEl });
    });
  } catch (err) {
    content.innerHTML = `<p class="text-red-500">Error: ${err instanceof Error ? err.message : "Error"}</p>`;
  }
}
