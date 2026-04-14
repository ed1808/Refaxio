import { renderSidebar } from "./sidebar";
import { renderHeader } from "./header";

export function renderLayout(): HTMLElement {
  const app = document.getElementById("app")!;

  app.innerHTML = `
    <div class="flex h-screen bg-gray-50">
      <aside id="sidebar" class="fixed inset-y-0 left-0 z-30 w-64 bg-white border-r border-gray-200 flex flex-col transform -translate-x-full lg:translate-x-0 lg:static transition-transform duration-200"></aside>
      <div class="flex-1 flex flex-col min-w-0">
        <header id="header"></header>
        <main id="content" class="flex-1 overflow-y-auto p-6"></main>
      </div>
    </div>
    <div id="sidebar-overlay" class="fixed inset-0 bg-black/30 z-20 hidden lg:hidden"></div>
  `;

  const sidebar = document.getElementById("sidebar")!;
  const header = document.getElementById("header")!;

  renderSidebar(sidebar);
  renderHeader(header);

  const overlay = document.getElementById("sidebar-overlay")!;
  sidebar.addEventListener("transitionend", () => {
    overlay.classList.toggle(
      "hidden",
      sidebar.classList.contains("-translate-x-full"),
    );
  });
  overlay.addEventListener("click", () => {
    sidebar.classList.add("-translate-x-full");
    overlay.classList.add("hidden");
  });

  document
    .getElementById("sidebar-toggle")
    ?.addEventListener("click", () => {
      overlay.classList.remove("hidden");
    });

  return document.getElementById("content")!;
}

export function getContent(): HTMLElement {
  return document.getElementById("content")!;
}
