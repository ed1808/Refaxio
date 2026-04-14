export interface FieldConfig {
  name: string;
  label: string;
  type?: "text" | "number" | "email" | "password" | "select" | "textarea";
  required?: boolean;
  options?: { value: string; label: string }[];
  placeholder?: string;
  value?: string | number;
  step?: string;
  min?: string;
  max?: string;
  maxLength?: number;
}

export function renderForm(
  fields: FieldConfig[],
  onSubmit: (data: Record<string, string>) => void | Promise<void>,
  submitLabel = "Guardar",
): HTMLFormElement {
  const form = document.createElement("form");
  form.className = "space-y-4";
  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    const formData = new FormData(form);
    const data: Record<string, string> = {};
    for (const [key, val] of formData.entries()) {
      data[key] = val.toString();
    }
    await onSubmit(data);
  });

  for (const field of fields) {
    const group = document.createElement("div");

    const label = document.createElement("label");
    label.className = "block text-sm font-medium text-gray-700 mb-1";
    label.textContent = field.label;
    if (field.required) {
      label.innerHTML += ' <span class="text-red-500">*</span>';
    }
    label.htmlFor = field.name;
    group.appendChild(label);

    const inputClass =
      "w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500";

    if (field.type === "select") {
      const select = document.createElement("select");
      select.name = field.name;
      select.id = field.name;
      select.className = inputClass;
      if (field.required) select.required = true;
      select.innerHTML = `<option value="">Seleccionar...</option>`;
      for (const opt of field.options ?? []) {
        const option = document.createElement("option");
        option.value = opt.value;
        option.textContent = opt.label;
        if (String(field.value) === opt.value) option.selected = true;
        select.appendChild(option);
      }
      group.appendChild(select);
    } else if (field.type === "textarea") {
      const textarea = document.createElement("textarea");
      textarea.name = field.name;
      textarea.id = field.name;
      textarea.className = inputClass;
      textarea.rows = 3;
      if (field.required) textarea.required = true;
      if (field.placeholder) textarea.placeholder = field.placeholder;
      if (field.value != null) textarea.value = String(field.value);
      group.appendChild(textarea);
    } else {
      const input = document.createElement("input");
      input.type = field.type ?? "text";
      input.name = field.name;
      input.id = field.name;
      input.className = inputClass;
      if (field.required) input.required = true;
      if (field.placeholder) input.placeholder = field.placeholder;
      if (field.value != null) input.value = String(field.value);
      if (field.step) input.step = field.step;
      if (field.min) input.min = field.min;
      if (field.max) input.max = field.max;
      if (field.maxLength) input.maxLength = field.maxLength;
      group.appendChild(input);
    }

    form.appendChild(group);
  }

  const submitBtn = document.createElement("button");
  submitBtn.type = "submit";
  submitBtn.className =
    "w-full px-4 py-2 text-sm font-medium text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 mt-2";
  submitBtn.textContent = submitLabel;
  form.appendChild(submitBtn);

  return form;
}
