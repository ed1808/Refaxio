import {
  Chart,
  BarController,
  BarElement,
  ArcElement,
  DoughnutController,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
  type ChartDataset,
} from "chart.js";

Chart.register(
  BarController,
  BarElement,
  ArcElement,
  DoughnutController,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
);

const registry = new Map<string, Chart>();

function destroyChart(canvasId: string): void {
  const existing = registry.get(canvasId);
  if (existing) {
    existing.destroy();
    registry.delete(canvasId);
  }
}

function getCanvas(canvasId: string): HTMLCanvasElement | null {
  return document.getElementById(canvasId) as HTMLCanvasElement | null;
}

const PALETTE = [
  "#6366f1", "#8b5cf6", "#ec4899", "#f59e0b", "#10b981",
  "#3b82f6", "#ef4444", "#14b8a6", "#f97316", "#a855f7",
];

export function renderBarChart(
  canvasId: string,
  labels: string[],
  data: number[],
  label: string,
  horizontal = false,
): void {
  destroyChart(canvasId);
  const canvas = getCanvas(canvasId);
  if (!canvas) return;

  const chart = new Chart(canvas, {
    type: "bar",
    data: {
      labels,
      datasets: [
        {
          label,
          data,
          backgroundColor: PALETTE[0],
          borderRadius: 4,
        },
      ],
    },
    options: {
      indexAxis: horizontal ? "y" : "x",
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { display: false },
        tooltip: { mode: "index" },
      },
      scales: {
        x: { grid: { display: !horizontal } },
        y: { grid: { display: horizontal } },
      },
    },
  });
  registry.set(canvasId, chart);
}

export function renderDoughnutChart(
  canvasId: string,
  labels: string[],
  data: number[],
  colors?: string[],
): void {
  destroyChart(canvasId);
  const canvas = getCanvas(canvasId);
  if (!canvas) return;

  const chart = new Chart(canvas, {
    type: "doughnut",
    data: {
      labels,
      datasets: [
        {
          data,
          backgroundColor: colors ?? [PALETTE[4], PALETTE[3]],
          borderWidth: 2,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { position: "bottom" },
        tooltip: { mode: "index" },
      },
    },
  });
  registry.set(canvasId, chart);
}

export function renderGroupedBarChart(
  canvasId: string,
  labels: string[],
  datasets: Array<{ label: string; data: number[]; color?: string }>,
): void {
  destroyChart(canvasId);
  const canvas = getCanvas(canvasId);
  if (!canvas) return;

  const chartDatasets: ChartDataset<"bar">[] = datasets.map((ds, i) => ({
    label: ds.label,
    data: ds.data,
    backgroundColor: ds.color ?? PALETTE[i % PALETTE.length],
    borderRadius: 4,
  }));

  const chart = new Chart(canvas, {
    type: "bar",
    data: { labels, datasets: chartDatasets },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { position: "top" },
        tooltip: { mode: "index" },
      },
      scales: {
        x: { grid: { display: false } },
        y: { beginAtZero: true },
      },
    },
  });
  registry.set(canvasId, chart);
}
