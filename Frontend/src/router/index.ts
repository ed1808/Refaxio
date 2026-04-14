import { isAuthenticated, getUserRole } from "../auth";

interface Route {
  path: string;
  handler: () => void | Promise<void>;
  roles?: string[];
}

const routes: Route[] = [];

export function registerRoute(
  path: string,
  handler: () => void | Promise<void>,
  roles?: string[],
): void {
  routes.push({ path, handler, roles });
}

export function navigate(hash: string): void {
  location.hash = hash;
}

function getHash(): string {
  return location.hash.slice(1) || "/login";
}

function matchRoute(hash: string): Route | undefined {
  return routes.find((r) => r.path === hash);
}

async function handleRoute(): Promise<void> {
  const hash = getHash();
  const route = matchRoute(hash);

  if (!route) {
    if (isAuthenticated()) {
      navigate("#/dashboard");
    } else {
      navigate("#/login");
    }
    return;
  }

  if (route.path !== "/login" && !isAuthenticated()) {
    navigate("#/login");
    return;
  }

  if (route.roles && route.roles.length > 0) {
    const role = getUserRole();
    if (!route.roles.includes(role)) {
      const content = document.getElementById("content");
      if (content) {
        content.innerHTML = `
          <div class="flex items-center justify-center h-full">
            <div class="text-center">
              <h1 class="text-4xl font-bold text-gray-400 mb-2">403</h1>
              <p class="text-gray-500">No tienes permisos para acceder a esta sección</p>
            </div>
          </div>`;
      }
      return;
    }
  }

  await route.handler();
}

export function initRouter(): void {
  window.addEventListener("hashchange", () => handleRoute());
  handleRoute();
}
