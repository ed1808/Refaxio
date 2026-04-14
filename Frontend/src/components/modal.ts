export interface ModalOptions {
  title: string;
  content: HTMLElement;
  onClose?: () => void;
}

let overlay: HTMLElement | null = null;

export function openModal(options: ModalOptions): void {
  closeModal();

  overlay = document.createElement("div");
  overlay.className =
    "fixed inset-0 z-40 flex items-center justify-center bg-black/50 p-4";
  overlay.addEventListener("click", (e) => {
    if (e.target === overlay) closeModal(options.onClose);
  });

  const modal = document.createElement("div");
  modal.className =
    "bg-white rounded-xl shadow-2xl w-full max-w-lg max-h-[90vh] flex flex-col";

  const header = document.createElement("div");
  header.className = "flex items-center justify-between px-6 py-4 border-b border-gray-200";
  header.innerHTML = `<h2 class="text-lg font-semibold text-gray-900">${options.title}</h2>`;

  const closeBtn = document.createElement("button");
  closeBtn.className =
    "text-gray-400 hover:text-gray-600 text-xl leading-none";
  closeBtn.innerHTML = "&times;";
  closeBtn.addEventListener("click", () => closeModal(options.onClose));
  header.appendChild(closeBtn);

  const body = document.createElement("div");
  body.className = "px-6 py-4 overflow-y-auto";
  body.appendChild(options.content);

  modal.appendChild(header);
  modal.appendChild(body);
  overlay.appendChild(modal);
  document.body.appendChild(overlay);
}

export function closeModal(onClose?: () => void): void {
  if (overlay) {
    overlay.remove();
    overlay = null;
  }
  onClose?.();
}
