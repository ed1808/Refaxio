import { openModal, closeModal } from "./modal";

export function confirmDialog(
  message: string,
  onConfirm: () => void | Promise<void>,
): void {
  const content = document.createElement("div");
  content.innerHTML = `<p class="text-gray-600 mb-6">${message}</p>`;

  const actions = document.createElement("div");
  actions.className = "flex justify-end gap-3";

  const cancelBtn = document.createElement("button");
  cancelBtn.className =
    "px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200";
  cancelBtn.textContent = "Cancelar";
  cancelBtn.addEventListener("click", () => closeModal());

  const confirmBtn = document.createElement("button");
  confirmBtn.className =
    "px-4 py-2 text-sm font-medium text-white bg-red-600 rounded-lg hover:bg-red-700";
  confirmBtn.textContent = "Eliminar";
  confirmBtn.addEventListener("click", async () => {
    closeModal();
    await onConfirm();
  });

  actions.appendChild(cancelBtn);
  actions.appendChild(confirmBtn);
  content.appendChild(actions);

  openModal({ title: "Confirmar acción", content });
}
