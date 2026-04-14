import "./style.css";
import { registerRoute, initRouter } from "./router";
import { renderLayout } from "./components/layout";
import { isAuthenticated } from "./auth";

// ── Pages ──
import { loginPage } from "./pages/login";
import { dashboardPage } from "./pages/dashboard";
import { categoriesPage } from "./pages/categories";
import { rolesPage } from "./pages/roles";
import { documentIdTypesPage } from "./pages/document-id-types";
import { personTypesPage } from "./pages/person-types";
import { storagesPage } from "./pages/storages";
import { productsPage } from "./pages/products";
import { customersPage } from "./pages/customers";
import { providersPage } from "./pages/providers";
import { usersPage } from "./pages/users";
import { inventoryPage } from "./pages/inventory";
import { inventoryMovementsPage } from "./pages/inventory-movements";
import { purchasesPage } from "./pages/purchases";
import { salesPage } from "./pages/sales";
import { transfersPage } from "./pages/transfers";
import { reportsPage } from "./pages/reports";

// ── Login (no layout) ──
registerRoute("/login", loginPage);

// ── Layout wrapper for authenticated pages ──
function withLayout(handler: () => void | Promise<void>) {
  return () => {
    if (!document.getElementById("sidebar")) {
      renderLayout();
    }
    return handler();
  };
}

// ── Dashboard ──
registerRoute("/dashboard", withLayout(dashboardPage));

// ── Catálogos ──
registerRoute("/categories", withLayout(categoriesPage));
registerRoute("/products", withLayout(productsPage));
registerRoute("/storages", withLayout(storagesPage));

// ── Personas ──
registerRoute("/customers", withLayout(customersPage));
registerRoute("/providers", withLayout(providersPage));

// ── Inventario ──
registerRoute("/inventory", withLayout(inventoryPage));
registerRoute("/inventory-movements", withLayout(inventoryMovementsPage));

// ── Operaciones ──
registerRoute("/purchases", withLayout(purchasesPage));
registerRoute("/sales", withLayout(salesPage));
registerRoute("/transfers", withLayout(transfersPage));

// ── Administración (Admin only) ──
registerRoute("/users", withLayout(usersPage), ["Admin"]);
registerRoute("/roles", withLayout(rolesPage), ["Admin"]);
registerRoute("/document-id-types", withLayout(documentIdTypesPage), ["Admin"]);
registerRoute("/person-types", withLayout(personTypesPage), ["Admin"]);

// ── Reportes (Admin, Director) ──
registerRoute("/reports", withLayout(reportsPage), ["Admin", "Director"]);

// ── Start ──
if (!isAuthenticated() && !location.hash) {
  location.hash = "#/login";
}
initRouter();
