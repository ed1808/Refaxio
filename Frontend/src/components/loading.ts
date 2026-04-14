export function showLoading(container: HTMLElement): void {
  container.innerHTML = `
    <div class="flex items-center justify-center py-20">
      <div class="animate-spin rounded-full h-10 w-10 border-4 border-indigo-500 border-t-transparent"></div>
    </div>`;
}
