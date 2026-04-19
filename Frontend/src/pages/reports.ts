import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderPagination } from "../components/pagination";
import { showToast } from "../components/toast";
import { reportApi } from "../api/report.api";
import { formatCurrency, formatDate } from "../utils/format";
import { renderBarChart, renderGroupedBarChart } from "../utils/charts";

type ReportTab = {
  id: string;
  label: string;
  hasDateFilter: boolean;
  load: (params: { startDate?: string; endDate?: string; page: number }) => Promise<void>;
};

let currentContainer: HTMLElement;
let chartContainer: HTMLElement;

function renderReportTable(headers: string[], rows: string[][]): string {
  return `
    <table class="w-full text-sm">
      <thead class="bg-gray-50">
        <tr>${headers.map((h) => `<th class="px-4 py-2 text-left font-medium text-gray-600">${h}</th>`).join("")}</tr>
      </thead>
      <tbody>
        ${rows.length === 0 ? `<tr><td colspan="${headers.length}" class="px-4 py-8 text-center text-gray-400">Sin datos</td></tr>` : rows.map((r) => `<tr class="border-t">${r.map((c) => `<td class="px-4 py-2">${c}</td>`).join("")}</tr>`).join("")}
      </tbody>
    </table>
  `;
}

function buildTabs(tabs: ReportTab[]): void {
  currentContainer = document.getElementById("report-content")!;
  chartContainer = document.getElementById("report-chart-container")!;
  const tabBar = document.getElementById("report-tabs")!;
  tabBar.innerHTML = "";

  tabs.forEach((tab, idx) => {
    const btn = document.createElement("button");
    btn.className = `px-4 py-2 text-sm font-medium rounded-t-lg border-b-2 ${idx === 0 ? "border-indigo-600 text-indigo-600" : "border-transparent text-gray-500 hover:text-gray-700"}`;
    btn.textContent = tab.label;
    btn.dataset.tabId = tab.id;
    btn.addEventListener("click", () => {
      tabBar.querySelectorAll("button").forEach((b) => {
        b.className = "px-4 py-2 text-sm font-medium rounded-t-lg border-b-2 border-transparent text-gray-500 hover:text-gray-700";
      });
      btn.className = "px-4 py-2 text-sm font-medium rounded-t-lg border-b-2 border-indigo-600 text-indigo-600";
      activateTab(tab);
    });
    tabBar.appendChild(btn);
  });

  activateTab(tabs[0]);
}

function activateTab(tab: ReportTab): void {
  const filterArea = document.getElementById("report-filters")!;
  if (tab.hasDateFilter) {
    filterArea.innerHTML = `
      <div class="flex items-end gap-3 mb-4">
        <div>
          <label class="text-xs text-gray-500">Desde</label>
          <input type="date" id="filter-start" class="px-3 py-1.5 border border-gray-300 rounded text-sm" />
        </div>
        <div>
          <label class="text-xs text-gray-500">Hasta</label>
          <input type="date" id="filter-end" class="px-3 py-1.5 border border-gray-300 rounded text-sm" />
        </div>
        <button id="filter-apply" class="px-4 py-1.5 bg-indigo-600 text-white text-sm rounded hover:bg-indigo-700">Filtrar</button>
      </div>
    `;
    document.getElementById("filter-apply")!.addEventListener("click", () => {
      const startDate = (document.getElementById("filter-start") as HTMLInputElement).value || undefined;
      const endDate = (document.getElementById("filter-end") as HTMLInputElement).value || undefined;
      tab.load({ startDate, endDate, page: 1 });
    });
  } else {
    filterArea.innerHTML = "";
  }
  tab.load({ page: 1 });
}

function showPagination(totalPages: number, currentPage: number, onPageChange: (p: number) => void): void {
  let paginationEl = document.getElementById("report-pagination");
  if (!paginationEl) {
    paginationEl = document.createElement("div");
    paginationEl.id = "report-pagination";
    currentContainer.parentElement?.appendChild(paginationEl);
  }
  renderPagination(paginationEl, { page: currentPage, totalPages, onPageChange });
}

const reportTabs: ReportTab[] = [
  {
    id: "top-sold",
    label: "Más Vendidos",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.topSoldProducts({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-top-sold"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Marca", "Categoría", "Cant. Vendida", "Valor Total"],
          res.items.map((i) => [i.sku, i.productName, i.brand, i.categoryName, String(i.totalQuantitySold), formatCurrency(i.totalSalesValue)]),
        );
        renderBarChart("chart-top-sold", res.items.map((i) => i.productName), res.items.map((i) => i.totalQuantitySold), "Cant. Vendida", true);
        showPagination(res.totalPages, res.page, (p) => reportTabs[0].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "top-purchased",
    label: "Más Comprados",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.topPurchasedProducts({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-top-purchased"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Marca", "Categoría", "Cant. Comprada", "Valor Total"],
          res.items.map((i) => [i.sku, i.productName, i.brand, i.categoryName, String(i.totalQuantityPurchased), formatCurrency(i.totalPurchaseValue)]),
        );
        renderBarChart("chart-top-purchased", res.items.map((i) => i.productName), res.items.map((i) => i.totalQuantityPurchased), "Cant. Comprada", true);
        showPagination(res.totalPages, res.page, (p) => reportTabs[1].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "low-rotation",
    label: "Baja Rotación",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.lowRotationProducts({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-low-rotation"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Marca", "Categoría", "Cant. Vendida", "Última Venta"],
          res.items.map((i) => [i.sku, i.productName, i.brand, i.categoryName, String(i.totalQuantitySold), i.lastSaleDate ? formatDate(i.lastSaleDate) : "—"]),
        );
        renderBarChart("chart-low-rotation", res.items.map((i) => i.productName), res.items.map((i) => i.totalQuantitySold), "Cant. Vendida");
        showPagination(res.totalPages, res.page, (p) => reportTabs[2].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "low-stock",
    label: "Stock Bajo",
    hasDateFilter: false,
    load: async ({ page }) => {
      try {
        const res = await reportApi.lowStockProducts({ page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-low-stock"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Almacén", "Stock", "Stock Mín."],
          res.items.map((i) => [i.productSku, i.productName, i.storageName, `<span class="${i.stock < i.minStock ? "text-red-600 font-semibold" : ""}">${i.stock}</span>`, String(i.minStock)]),
        );
        renderGroupedBarChart(
          "chart-low-stock",
          res.items.map((i) => `${i.productName} (${i.storageName})`),
          [
            { label: "Stock Actual", data: res.items.map((i) => i.stock), color: "#6366f1" },
            { label: "Stock Mín.", data: res.items.map((i) => i.minStock), color: "#f59e0b" },
          ],
        );
        showPagination(res.totalPages, res.page, (p) => reportTabs[3].load({ page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "sales-value",
    label: "Valor Ventas",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.productsBySalesValue({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-sales-value"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Marca", "Categoría", "Valor Ventas"],
          res.items.map((i) => [i.sku, i.productName, i.brand, i.categoryName, formatCurrency(i.totalSalesValue)]),
        );
        renderBarChart("chart-sales-value", res.items.map((i) => i.productName), res.items.map((i) => i.totalSalesValue), "Valor Ventas", true);
        showPagination(res.totalPages, res.page, (p) => reportTabs[4].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "inventory-value",
    label: "Valor Inventario",
    hasDateFilter: false,
    load: async ({ page }) => {
      try {
        const res = await reportApi.productsByInventoryValue({ page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-inventory-value"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["SKU", "Producto", "Marca", "Almacén", "Stock", "Precio", "Valor Total"],
          res.items.map((i) => [i.productSku, i.productName, i.brand, i.storageName, String(i.stock), formatCurrency(i.unitPrice), formatCurrency(i.totalValue)]),
        );
        renderBarChart("chart-inventory-value", res.items.map((i) => i.productName), res.items.map((i) => i.totalValue), "Valor Total", true);
        showPagination(res.totalPages, res.page, (p) => reportTabs[5].load({ page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "top-customers",
    label: "Top Clientes",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.topCustomers({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-top-customers"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["Cliente", "Documento", "Nro. Ventas", "Monto Total"],
          res.items.map((i) => [i.fullName, i.documentIdNumber, String(i.totalSalesCount), formatCurrency(i.totalAmount)]),
        );
        renderBarChart("chart-top-customers", res.items.map((i) => i.fullName), res.items.map((i) => i.totalAmount), "Monto Total");
        showPagination(res.totalPages, res.page, (p) => reportTabs[6].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
  {
    id: "top-sellers",
    label: "Top Vendedores",
    hasDateFilter: true,
    load: async ({ startDate, endDate, page }) => {
      try {
        const res = await reportApi.topSellers({ startDate, endDate, page, pageSize: 10 });
        chartContainer.innerHTML = `<div class="relative h-56"><canvas id="chart-top-sellers"></canvas></div>`;
        currentContainer.innerHTML = renderReportTable(
          ["Vendedor", "Usuario", "Nro. Ventas", "Monto Total"],
          res.items.map((i) => [i.fullName, i.username, String(i.totalSalesCount), formatCurrency(i.totalAmount)]),
        );
        renderBarChart("chart-top-sellers", res.items.map((i) => i.fullName), res.items.map((i) => i.totalAmount), "Monto Total");
        showPagination(res.totalPages, res.page, (p) => reportTabs[7].load({ startDate, endDate, page: p }));
      } catch (err) { showToast(err instanceof Error ? err.message : "Error", "error"); }
    },
  },
];

export async function reportsPage(): Promise<void> {
  const content = getContent();
  showLoading(content);

  content.innerHTML = `
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-gray-900">Reportes</h1>
    </div>
    <div class="bg-white rounded-xl border border-gray-200 overflow-hidden">
      <div id="report-tabs" class="flex flex-wrap gap-1 border-b bg-gray-50 px-2 pt-2"></div>
      <div class="p-4">
        <div id="report-filters"></div>
        <div id="report-chart-container" class="mb-4"></div>
        <div id="report-content"></div>
      </div>
    </div>
  `;

  buildTabs(reportTabs);
}
