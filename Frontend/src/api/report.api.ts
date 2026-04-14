import { get } from "./client";
import type {
  PaginatedResponse,
  TopSoldProduct,
  TopPurchasedProduct,
  LowRotationProduct,
  LowStockProduct,
  ProductSalesValue,
  ProductInventoryValue,
  TopCustomer,
  TopSeller,
  CustomerProductDetail,
  SellerProductDetail,
} from "../types";

function qs(params: Record<string, string | number | undefined>): string {
  const entries = Object.entries(params).filter(([, v]) => v != null && v !== "");
  return entries.length ? "?" + new URLSearchParams(entries.map(([k, v]) => [k, String(v)])).toString() : "";
}

export const reportApi = {
  topSoldProducts: (p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<TopSoldProduct>>(`/report/top-sold-products${qs(p)}`),

  topPurchasedProducts: (p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<TopPurchasedProduct>>(`/report/top-purchased-products${qs(p)}`),

  lowRotationProducts: (p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<LowRotationProduct>>(`/report/low-rotation-products${qs(p)}`),

  lowStockProducts: (p: { page?: number; pageSize?: number }) =>
    get<PaginatedResponse<LowStockProduct>>(`/report/low-stock-products${qs(p)}`),

  productsBySalesValue: (p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<ProductSalesValue>>(`/report/products-by-sales-value${qs(p)}`),

  productsByInventoryValue: (p: { page?: number; pageSize?: number }) =>
    get<PaginatedResponse<ProductInventoryValue>>(`/report/products-by-inventory-value${qs(p)}`),

  topCustomers: (p: { startDate?: string; endDate?: string; customerId?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<TopCustomer>>(`/report/top-customers${qs(p)}`),

  topSellers: (p: { startDate?: string; endDate?: string; userId?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<TopSeller>>(`/report/top-sellers${qs(p)}`),

  customerDetail: (customerId: string, p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<CustomerProductDetail>>(`/report/customers/${customerId}/detail${qs(p)}`),

  sellerDetail: (userId: string, p: { startDate?: string; endDate?: string; page?: number; pageSize?: number }) =>
    get<PaginatedResponse<SellerProductDetail>>(`/report/sellers/${userId}/detail${qs(p)}`),
};
