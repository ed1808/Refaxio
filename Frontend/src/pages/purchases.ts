import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderTable, type Column } from "../components/table";
import { openModal, closeModal } from "../components/modal";
import { confirmDialog } from "../components/confirm-dialog";
import { showToast } from "../components/toast";
import { purchaseApi } from "../api/purchase.api";
import { providerApi } from "../api/provider.api";
import { productApi } from "../api/product.api";
import { storageApi } from "../api/storage.api";
import { getUserId } from "../auth";
import type { PurchaseResponse, PurchaseDetailCreate, ProviderResponse, ProductResponse, StorageResponse } from "../types";
import { formatCurrency, formatDateTime } from "../utils/format";

function statusBadge(status: string): string {
  const colors: Record<string, string> = {
    RECEIVED: "bg-green-100 text-green-700",
    PENDING: "bg-yellow-100 text-yellow-700",
    COMPLETED: "bg-blue-100 text-blue-700",
    CANCELED: "bg-red-100 text-red-700",
  };
  const color = colors[status] ?? "bg-gray-100 text-gray-700";
  return `<span class="px-2 py-0.5 rounded-full text-xs font-medium ${color}">${status}</span>`;
}

function buildCreateForm(
  providers: ProviderResponse[],
  products: ProductResponse[],
  storages: StorageResponse[],
  onSubmit: (data: { providerInvoiceNumber: string; providerId: string; details: PurchaseDetailCreate[] }) => Promise<void>,
): HTMLElement {
  const wrapper = document.createElement("div");

  wrapper.innerHTML = `
    <form id="purchase-form" class="space-y-4">
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Nro. Factura Proveedor <span class="text-red-500">*</span></label>
        <input name="providerInvoiceNumber" required maxlength="50" class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500" />
      </div>
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Proveedor <span class="text-red-500">*</span></label>
        <select name="providerId" required class="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500">
          <option value="">Seleccionar...</option>
          ${providers.map((p) => `<option value="${p.id}">${p.firstName} ${p.firstSurname ?? ""} — ${p.documentIdNumber}</option>`).join("")}
        </select>
      </div>
      <div class="border-t pt-4">
        <div class="flex items-center justify-between mb-2">
          <h3 class="text-sm font-semibold text-gray-900">Detalle</h3>
          <button type="button" id="add-line" class="text-xs text-indigo-600 hover:text-indigo-800 font-medium">+ Agregar línea</button>
        </div>
        <div id="detail-lines" class="space-y-3"></div>
      </div>
      <button type="submit" class="w-full px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 mt-2">Guardar Compra</button>
    </form>
  `;

  const linesContainer = wrapper.querySelector("#detail-lines")!;
  let lineCount = 0;

  function addLine(): void {
    const idx = lineCount++;
    const line = document.createElement("div");
    line.className = "grid grid-cols-6 gap-2 items-end bg-gray-50 p-3 rounded-lg";
    line.dataset.lineIdx = String(idx);
    line.innerHTML = `
      <div class="col-span-2">
        <label class="text-xs text-gray-500">Producto</label>
        <select name="detail_product_${idx}" required class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs">
          <option value="">Seleccionar</option>
          ${products.map((p) => `<option value="${p.sku}" data-price="${p.purchasePrice}">${p.sku} — ${p.productName}</option>`).join("")}
        </select>
      </div>
      <div>
        <label class="text-xs text-gray-500">Almacén</label>
        <select name="detail_storage_${idx}" required class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs">
          <option value="">Sel.</option>
          ${storages.map((s) => `<option value="${s.id}">${s.storageName}</option>`).join("")}
        </select>
      </div>
      <div>
        <label class="text-xs text-gray-500">Cantidad</label>
        <input name="detail_qty_${idx}" type="number" min="1" required value="1" class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs" />
      </div>
      <div>
        <label class="text-xs text-gray-500">Costo Unit.</label>
        <input name="detail_cost_${idx}" type="number" min="0" step="0.01" required class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs" />
      </div>
      <div class="flex items-end gap-1">
        <div class="flex-1">
          <label class="text-xs text-gray-500">Impuesto</label>
          <input name="detail_tax_${idx}" type="number" min="0" step="0.01" value="0" required class="w-full px-2 py-1.5 border border-gray-300 rounded text-xs" />
        </div>
        <button type="button" class="remove-line text-red-500 hover:text-red-700 pb-1.5 text-sm">&times;</button>
      </div>
    `;

    const productSelect = line.querySelector(`[name="detail_product_${idx}"]`) as HTMLSelectElement;
    const costInput = line.querySelector(`[name="detail_cost_${idx}"]`) as HTMLInputElement;
    productSelect.addEventListener("change", () => {
      const option = productSelect.selectedOptions[0];
      const price = option?.dataset.price;
      if (price) costInput.value = price;
    });

    line.querySelector(".remove-line")!.addEventListener("click", () => line.remove());
    linesContainer.appendChild(line);
  }

  wrapper.querySelector("#add-line")!.addEventListener("click", addLine);
  addLine();

  const form = wrapper.querySelector("#purchase-form") as HTMLFormElement;
  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const fd = new FormData(form);
    const details: PurchaseDetailCreate[] = [];
    const lines = linesContainer.querySelectorAll("[data-line-idx]");
    for (const line of lines) {
      const idx = line.getAttribute("data-line-idx")!;
      const qty = parseInt(fd.get(`detail_qty_${idx}`) as string);
      const cost = parseFloat(fd.get(`detail_cost_${idx}`) as string);
      const tax = parseFloat(fd.get(`detail_tax_${idx}`) as string);
      details.push({
        productSku: fd.get(`detail_product_${idx}`) as string,
        storageId: parseInt(fd.get(`detail_storage_${idx}`) as string),
        quantity: qty,
        unitCost: cost,
        taxAmount: tax,
        subtotal: qty * cost + tax,
      });
    }
    if (details.length === 0) {
      showToast("Agrega al menos una línea de detalle", "error");
      return;
    }
    await onSubmit({
      providerInvoiceNumber: fd.get("providerInvoiceNumber") as string,
      providerId: fd.get("providerId") as string,
      details,
    });
  });

  return wrapper;
}

function buildDetailView(purchase: PurchaseResponse): HTMLElement {
  const el = document.createElement("div");
  el.innerHTML = `
    <div class="space-y-3 text-sm">
      <div class="grid grid-cols-2 gap-4 mb-4">
        <div><span class="text-gray-500">Factura:</span> <strong>${purchase.providerInvoiceNumber}</strong></div>
        <div><span class="text-gray-500">Proveedor:</span> <strong>${purchase.providerName}</strong></div>
        <div><span class="text-gray-500">Usuario:</span> <strong>${purchase.username}</strong></div>
        <div><span class="text-gray-500">Estado:</span> ${statusBadge(purchase.status)}</div>
        <div><span class="text-gray-500">Total:</span> <strong>${formatCurrency(purchase.totalAmount)}</strong></div>
        <div><span class="text-gray-500">Fecha:</span> ${formatDateTime(purchase.createdAt)}</div>
      </div>
      <table class="w-full text-xs">
        <thead class="bg-gray-50"><tr>
          <th class="px-3 py-2 text-left">Producto</th>
          <th class="px-3 py-2 text-left">Almacén</th>
          <th class="px-3 py-2 text-right">Cant.</th>
          <th class="px-3 py-2 text-right">Costo</th>
          <th class="px-3 py-2 text-right">Impuesto</th>
          <th class="px-3 py-2 text-right">Subtotal</th>
        </tr></thead>
        <tbody>
          ${purchase.details.map((d) => `
            <tr class="border-t">
              <td class="px-3 py-2">${d.productName} (${d.productSku})</td>
              <td class="px-3 py-2">${d.storageName}</td>
              <td class="px-3 py-2 text-right">${d.quantity}</td>
              <td class="px-3 py-2 text-right">${formatCurrency(d.unitCost)}</td>
              <td class="px-3 py-2 text-right">${formatCurrency(d.taxAmount)}</td>
              <td class="px-3 py-2 text-right">${formatCurrency(d.subtotal)}</td>
            </tr>`).join("")}
        </tbody>
      </table>
    </div>
  `;
  return el;
}

export async function purchasesPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  try {
    const data = await purchaseApi.getAll();

    const columns: Column<PurchaseResponse>[] = [
      { key: "providerInvoiceNumber", label: "Nro. Factura" },
      { key: "providerName", label: "Proveedor" },
      { key: "username", label: "Usuario" },
      { key: "totalAmount", label: "Total", render: (i) => formatCurrency(i.totalAmount) },
      { key: "status", label: "Estado", render: (i) => statusBadge(i.status) },
      { key: "createdAt", label: "Fecha", render: (i) => formatDateTime(i.createdAt) },
    ];

    content.innerHTML = `
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Compras</h1>
        <button id="create-btn" class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700">+ Nueva Compra</button>
      </div>
      <div class="bg-white rounded-xl border border-gray-200" id="table-container"></div>
    `;

    renderTable(document.getElementById("table-container")!, columns, data, [
      {
        label: "Ver",
        variant: "primary",
        onClick: async (item) => {
          const purchase = await purchaseApi.getById(item.id);
          openModal({ title: `Compra ${purchase.providerInvoiceNumber}`, content: buildDetailView(purchase) });
        },
      },
      {
        label: "Eliminar",
        variant: "danger",
        onClick: (item) => {
          confirmDialog("¿Eliminar esta compra?", async () => {
            try {
              await purchaseApi.remove(item.id);
              showToast("Compra eliminada", "success");
              await purchasesPage();
            } catch (err) {
              showToast(err instanceof Error ? err.message : "Error", "error");
            }
          });
        },
      },
    ]);

    document.getElementById("create-btn")!.addEventListener("click", async () => {
      const [providers, products, storages] = await Promise.all([
        providerApi.getAll(),
        productApi.getAll(),
        storageApi.getAll(),
      ]);
      const formEl = buildCreateForm(providers, products, storages, async (formData) => {
        try {
          await purchaseApi.create({
            ...formData,
            userId: getUserId(),
            status: "RECEIVED",
          });
          closeModal();
          showToast("Compra creada", "success");
          await purchasesPage();
        } catch (err) {
          showToast(err instanceof Error ? err.message : "Error", "error");
        }
      });
      openModal({ title: "Nueva Compra", content: formEl });
    });
  } catch (err) {
    content.innerHTML = `<p class="text-red-500">Error: ${err instanceof Error ? err.message : "Error"}</p>`;
  }
}
