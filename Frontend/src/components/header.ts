import { getUsername, getUserRole, removeToken } from "../auth";
import { navigate } from "../router";

export function renderHeader(container: HTMLElement): void {
  const username = getUsername();
  const role = getUserRole();

  container.innerHTML = `
    <div class="flex items-center justify-between px-6 h-16 border-b border-gray-200 bg-white">
      <button id="sidebar-toggle" class="lg:hidden text-gray-500 hover:text-gray-700">
        <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
        </svg>
      </button>
      <div class="flex-1"></div>
      <div class="flex items-center gap-4">
        <div class="text-right">
          <p class="text-sm font-medium text-gray-900">${username}</p>
          <p class="text-xs text-gray-500">${role}</p>
        </div>
        <button id="logout-btn" class="text-sm text-gray-500 hover:text-red-600 transition-colors" title="Cerrar sesión">
          <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/>
          </svg>
        </button>
      </div>
    </div>
  `;

  container.querySelector("#logout-btn")!.addEventListener("click", () => {
    removeToken();
    navigate("#/login");
  });

  container.querySelector("#sidebar-toggle")?.addEventListener("click", () => {
    const sidebar = document.getElementById("sidebar");
    sidebar?.classList.toggle("-translate-x-full");
  });
}
