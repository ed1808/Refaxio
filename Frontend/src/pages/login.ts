import { login } from "../api/auth.api";
import { setToken, isAuthenticated } from "../auth";
import { navigate } from "../router";
import { showToast } from "../components/toast";

export function loginPage(): void {
  if (isAuthenticated()) {
    navigate("#/dashboard");
    return;
  }

  const app = document.getElementById("app")!;
  app.innerHTML = `
    <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-100 via-white to-purple-100 px-4">
      <div class="w-full max-w-md">
        <div class="bg-white rounded-2xl shadow-xl p-8">
          <div class="text-center mb-8">
            <div class="w-14 h-14 bg-indigo-600 rounded-xl flex items-center justify-center mx-auto mb-4">
              <span class="text-white font-bold text-2xl">R</span>
            </div>
            <h1 class="text-2xl font-bold text-gray-900">Refaxio</h1>
            <p class="text-gray-500 text-sm mt-1">Inicia sesión para continuar</p>
          </div>
          <form id="login-form" class="space-y-5">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1" for="username">Usuario</label>
              <input id="username" name="username" type="text" required autocomplete="username"
                class="w-full px-4 py-2.5 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                placeholder="Tu nombre de usuario" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1" for="password">Contraseña</label>
              <input id="password" name="password" type="password" required autocomplete="current-password"
                class="w-full px-4 py-2.5 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                placeholder="Tu contraseña" />
            </div>
            <button type="submit" id="login-btn"
              class="w-full py-2.5 px-4 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
              Iniciar sesión
            </button>
          </form>
        </div>
      </div>
    </div>
  `;

  const form = document.getElementById("login-form") as HTMLFormElement;
  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const btn = document.getElementById("login-btn") as HTMLButtonElement;
    const username = (document.getElementById("username") as HTMLInputElement).value;
    const password = (document.getElementById("password") as HTMLInputElement).value;

    btn.disabled = true;
    btn.textContent = "Ingresando...";

    try {
      const res = await login({ username, password });
      setToken(res.token);
      navigate("#/dashboard");
    } catch (err) {
      showToast(err instanceof Error ? err.message : "Error al iniciar sesión", "error");
    } finally {
      btn.disabled = false;
      btn.textContent = "Iniciar sesión";
    }
  });
}
