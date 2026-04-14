export interface PaginationOptions {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export function renderPagination(
  container: HTMLElement,
  opts: PaginationOptions,
): void {
  if (opts.totalPages <= 1) {
    container.innerHTML = "";
    return;
  }

  const wrapper = document.createElement("div");
  wrapper.className = "flex items-center justify-between mt-4 text-sm";

  const info = document.createElement("span");
  info.className = "text-gray-500";
  info.textContent = `Página ${opts.page} de ${opts.totalPages}`;

  const btns = document.createElement("div");
  btns.className = "flex gap-2";

  const prevBtn = document.createElement("button");
  prevBtn.className =
    "px-3 py-1 rounded border border-gray-300 text-gray-600 hover:bg-gray-50 disabled:opacity-40 disabled:cursor-not-allowed";
  prevBtn.textContent = "Anterior";
  prevBtn.disabled = opts.page <= 1;
  prevBtn.addEventListener("click", () => opts.onPageChange(opts.page - 1));

  const nextBtn = document.createElement("button");
  nextBtn.className =
    "px-3 py-1 rounded border border-gray-300 text-gray-600 hover:bg-gray-50 disabled:opacity-40 disabled:cursor-not-allowed";
  nextBtn.textContent = "Siguiente";
  nextBtn.disabled = opts.page >= opts.totalPages;
  nextBtn.addEventListener("click", () => opts.onPageChange(opts.page + 1));

  btns.appendChild(prevBtn);
  btns.appendChild(nextBtn);

  wrapper.appendChild(info);
  wrapper.appendChild(btns);

  container.innerHTML = "";
  container.appendChild(wrapper);
}
