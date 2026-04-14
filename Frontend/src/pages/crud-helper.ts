import { getContent } from "../components/layout";
import { showLoading } from "../components/loading";
import { renderTable, type Column, type TableAction } from "../components/table";
import { openModal, closeModal } from "../components/modal";
import { confirmDialog } from "../components/confirm-dialog";
import { renderForm, type FieldConfig } from "../components/form-field";
import { showToast } from "../components/toast";

export interface CrudPageConfig<TResponse, TCreate, TUpdate> {
  title: string;
  entityName: string;
  columns: Column<TResponse>[];
  getAll: () => Promise<TResponse[]>;
  create: (data: TCreate) => Promise<TResponse>;
  update: (id: string | number, data: TUpdate) => Promise<TResponse>;
  remove: (id: string | number) => Promise<void>;
  getId: (item: TResponse) => string | number;
  getCreateFields: (lookups?: Record<string, unknown[]>) => FieldConfig[];
  getUpdateFields: (item: TResponse, lookups?: Record<string, unknown[]>) => FieldConfig[];
  mapFormToCreate: (data: Record<string, string>) => TCreate;
  mapFormToUpdate: (data: Record<string, string>) => TUpdate;
  loadLookups?: () => Promise<Record<string, unknown[]>>;
}

export function createCrudPage<TResponse, TCreate, TUpdate>(
  config: CrudPageConfig<TResponse, TCreate, TUpdate>,
): () => Promise<void> {
  return async () => {
    const content = getContent();
    showLoading(content);

    try {
      const [data, lookups] = await Promise.all([
        config.getAll(),
        config.loadLookups ? config.loadLookups() : Promise.resolve(undefined),
      ]);

      content.innerHTML = `
        <div class="flex items-center justify-between mb-6">
          <div>
            <h1 class="text-2xl font-bold text-gray-900">${config.title}</h1>
          </div>
          <button id="create-btn" class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 transition-colors">
            + Nuevo
          </button>
        </div>
        <div class="bg-white rounded-xl border border-gray-200" id="table-container"></div>
      `;

      const tableContainer = document.getElementById("table-container")!;
      const actions: TableAction<TResponse>[] = [
        {
          label: "Editar",
          variant: "primary",
          onClick: (item) => {
            const form = renderForm(
              config.getUpdateFields(item, lookups),
              async (formData) => {
                try {
                  await config.update(config.getId(item), config.mapFormToUpdate(formData));
                  closeModal();
                  showToast(`${config.entityName} actualizado`, "success");
                  await createCrudPage(config)();
                } catch (err) {
                  showToast(err instanceof Error ? err.message : "Error al actualizar", "error");
                }
              },
            );
            openModal({ title: `Editar ${config.entityName}`, content: form });
          },
        },
        {
          label: "Eliminar",
          variant: "danger",
          onClick: (item) => {
            confirmDialog(`¿Eliminar este ${config.entityName.toLowerCase()}?`, async () => {
              try {
                await config.remove(config.getId(item));
                showToast(`${config.entityName} eliminado`, "success");
                await createCrudPage(config)();
              } catch (err) {
                showToast(err instanceof Error ? err.message : "Error al eliminar", "error");
              }
            });
          },
        },
      ];

      renderTable(tableContainer, config.columns, data, actions);

      document.getElementById("create-btn")!.addEventListener("click", () => {
        const form = renderForm(
          config.getCreateFields(lookups),
          async (formData) => {
            try {
              await config.create(config.mapFormToCreate(formData));
              closeModal();
              showToast(`${config.entityName} creado`, "success");
              await createCrudPage(config)();
            } catch (err) {
              showToast(err instanceof Error ? err.message : "Error al crear", "error");
            }
          },
        );
        openModal({ title: `Nuevo ${config.entityName}`, content: form });
      });
    } catch (err) {
      content.innerHTML = `<p class="text-red-500">Error: ${err instanceof Error ? err.message : "Error desconocido"}</p>`;
    }
  };
}
