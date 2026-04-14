let toastContainer: HTMLElement | null = null;

function ensureContainer(): HTMLElement {
  if (!toastContainer) {
    toastContainer = document.createElement("div");
    toastContainer.className = "fixed top-4 right-4 z-50 flex flex-col gap-2";
    document.body.appendChild(toastContainer);
  }
  return toastContainer;
}

export function showToast(
  message: string,
  type: "success" | "error" | "info" = "info",
): void {
  const container = ensureContainer();
  const colors = {
    success: "bg-green-600",
    error: "bg-red-600",
    info: "bg-blue-600",
  };
  const toast = document.createElement("div");
  toast.className = `${colors[type]} text-white px-4 py-3 rounded-lg shadow-lg text-sm max-w-sm transform transition-all duration-300 translate-x-full`;
  toast.textContent = message;
  container.appendChild(toast);

  requestAnimationFrame(() => {
    toast.classList.remove("translate-x-full");
    toast.classList.add("translate-x-0");
  });

  setTimeout(() => {
    toast.classList.remove("translate-x-0");
    toast.classList.add("translate-x-full");
    setTimeout(() => toast.remove(), 300);
  }, 3000);
}
