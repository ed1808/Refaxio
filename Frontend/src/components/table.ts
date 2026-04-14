export interface Column<T> {
  key: string;
  label: string;
  render?: (item: T) => string | HTMLElement;
}

export interface TableAction<T> {
  label: string;
  icon?: string;
  variant?: "primary" | "danger" | "default";
  onClick: (item: T) => void;
}

export function renderTable<T>(
  container: HTMLElement,
  columns: Column<T>[],
  data: T[],
  actions?: TableAction<T>[],
): void {
  if (data.length === 0) {
    container.innerHTML = `
      <div class="text-center py-12 text-gray-500">
        <svg class="mx-auto h-12 w-12 text-gray-300 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-2.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
        </svg>
        <p>No hay datos para mostrar</p>
      </div>`;
    return;
  }

  const table = document.createElement("div");
  table.className = "overflow-x-auto";
  table.innerHTML = `
    <table class="min-w-full divide-y divide-gray-200">
      <thead class="bg-gray-50">
        <tr>
          ${columns.map((c) => `<th class="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wider">${c.label}</th>`).join("")}
          ${actions ? '<th class="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase tracking-wider">Acciones</th>' : ""}
        </tr>
      </thead>
      <tbody class="bg-white divide-y divide-gray-100"></tbody>
    </table>`;

  const tbody = table.querySelector("tbody")!;
  for (const item of data) {
    const tr = document.createElement("tr");
    tr.className = "hover:bg-gray-50 transition-colors";

    for (const col of columns) {
      const td = document.createElement("td");
      td.className = "px-4 py-3 text-sm text-gray-700 whitespace-nowrap";
      if (col.render) {
        const rendered = col.render(item);
        if (typeof rendered === "string") {
          td.innerHTML = rendered;
        } else {
          td.appendChild(rendered);
        }
      } else {
        td.textContent = String(
          (item as Record<string, unknown>)[col.key] ?? "—",
        );
      }
      tr.appendChild(td);
    }

    if (actions) {
      const td = document.createElement("td");
      td.className = "px-4 py-3 text-sm text-right whitespace-nowrap";
      for (const action of actions) {
        const btn = document.createElement("button");
        const variants: Record<string, string> = {
          primary: "text-indigo-600 hover:text-indigo-800",
          danger: "text-red-600 hover:text-red-800",
          default: "text-gray-600 hover:text-gray-800",
        };
        btn.className = `${variants[action.variant ?? "default"]} font-medium ml-3 text-xs`;
        btn.textContent = action.label;
        btn.addEventListener("click", () => action.onClick(item));
        td.appendChild(btn);
      }
      tr.appendChild(td);
    }

    tbody.appendChild(tr);
  }

  container.innerHTML = "";
  container.appendChild(table);
}
