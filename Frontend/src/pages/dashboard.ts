import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { productApi } from "../api/product.api";
import { storageApi } from "../api/storage.api";
import { saleApi } from "../api/sale.api";
import { purchaseApi } from "../api/purchase.api";
import { formatCurrency } from "../utils/format";

export async function dashboardPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  try {
    const [products, storages, sales, purchases] = await Promise.all([
      productApi.getAll(),
      storageApi.getAll(),
      saleApi.getAll(),
      purchaseApi.getAll(),
    ]);

    const totalSales = sales.reduce((sum, s) => sum + s.totalAmount, 0);
    const totalPurchases = purchases.reduce((sum, p) => sum + p.totalAmount, 0);

    content.innerHTML = `
      <div class="mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Dashboard</h1>
        <p class="text-gray-500 text-sm mt-1">Resumen general del sistema</p>
      </div>

      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm text-gray-500">Productos</p>
              <p class="text-2xl font-bold text-gray-900 mt-1">${products.length}</p>
            </div>
            <div class="w-10 h-10 bg-blue-100 rounded-lg flex items-center justify-center">
              <svg class="w-5 h-5 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"/></svg>
            </div>
          </div>
        </div>
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm text-gray-500">Almacenes</p>
              <p class="text-2xl font-bold text-gray-900 mt-1">${storages.length}</p>
            </div>
            <div class="w-10 h-10 bg-purple-100 rounded-lg flex items-center justify-center">
              <svg class="w-5 h-5 text-purple-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 14v3m4-3v3m4-3v3M3 21h18M3 10h18M3 7l9-4 9 4M4 10h16v11H4V10z"/></svg>
            </div>
          </div>
        </div>
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm text-gray-500">Total Ventas</p>
              <p class="text-2xl font-bold text-gray-900 mt-1">${formatCurrency(totalSales)}</p>
            </div>
            <div class="w-10 h-10 bg-green-100 rounded-lg flex items-center justify-center">
              <svg class="w-5 h-5 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
            </div>
          </div>
        </div>
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm text-gray-500">Total Compras</p>
              <p class="text-2xl font-bold text-gray-900 mt-1">${formatCurrency(totalPurchases)}</p>
            </div>
            <div class="w-10 h-10 bg-orange-100 rounded-lg flex items-center justify-center">
              <svg class="w-5 h-5 text-orange-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
            </div>
          </div>
        </div>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Últimas Ventas</h2>
          <div class="space-y-3">
            ${sales.slice(0, 5).map((s) => `
              <div class="flex items-center justify-between py-2 border-b border-gray-100 last:border-0">
                <div>
                  <p class="text-sm font-medium text-gray-900">${s.invoiceNumber}</p>
                  <p class="text-xs text-gray-500">${s.customerName}</p>
                </div>
                <span class="text-sm font-medium text-green-600">${formatCurrency(s.totalAmount)}</span>
              </div>
            `).join("")}
            ${sales.length === 0 ? '<p class="text-sm text-gray-500">Sin ventas registradas</p>' : ""}
          </div>
        </div>
        <div class="bg-white rounded-xl border border-gray-200 p-5">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Últimas Compras</h2>
          <div class="space-y-3">
            ${purchases.slice(0, 5).map((p) => `
              <div class="flex items-center justify-between py-2 border-b border-gray-100 last:border-0">
                <div>
                  <p class="text-sm font-medium text-gray-900">${p.providerInvoiceNumber}</p>
                  <p class="text-xs text-gray-500">${p.providerName}</p>
                </div>
                <span class="text-sm font-medium text-orange-600">${formatCurrency(p.totalAmount)}</span>
              </div>
            `).join("")}
            ${purchases.length === 0 ? '<p class="text-sm text-gray-500">Sin compras registradas</p>' : ""}
          </div>
        </div>
      </div>
    `;
  } catch (err) {
    content.innerHTML = `<p class="text-red-500">Error al cargar el dashboard: ${err instanceof Error ? err.message : "Error desconocido"}</p>`;
  }
}
