// ── Auth ──
export interface LoginRequest {
  username: string;
  password: string;
}
export interface AuthResponse {
  token: string;
  expiresAt: string;
}

// ── Category ──
export interface CategoryCreate {
  categoryName: string;
}
export interface CategoryUpdate {
  categoryName: string;
  active?: boolean;
}
export interface CategoryResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  categoryName: string;
}

// ── Role ──
export interface RoleCreate {
  roleName: string;
}
export interface RoleUpdate {
  roleName: string;
  active?: boolean;
}
export interface RoleResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  roleName: string;
}

// ── DocumentIdType ──
export interface DocumentIdTypeCreate {
  documentIdName: string;
}
export interface DocumentIdTypeUpdate {
  documentIdName: string;
  active?: boolean;
}
export interface DocumentIdTypeResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  documentIdName: string;
}

// ── PersonType ──
export interface PersonTypeCreate {
  personTypeName: string;
}
export interface PersonTypeUpdate {
  personTypeName: string;
  active?: boolean;
}
export interface PersonTypeResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  personTypeName: string;
}

// ── Storage ──
export interface StorageCreate {
  storageName: string;
  address?: string;
}
export interface StorageUpdate {
  storageName: string;
  address?: string;
  active?: boolean;
}
export interface StorageResponse {
  id: number;
  active?: boolean;
  createdAt?: string;
  storageName: string;
  address?: string;
}

// ── Product ──
export interface ProductCreate {
  sku: string;
  productName: string;
  productDescription?: string;
  purchasePrice: number;
  salePrice: number;
  brand: string;
  categoryId: string;
}
export interface ProductUpdate {
  productName: string;
  productDescription?: string;
  purchasePrice: number;
  salePrice: number;
  brand: string;
  categoryId: string;
  active?: boolean;
}
export interface ProductResponse {
  sku: string;
  active?: boolean;
  createdAt?: string;
  productName: string;
  productDescription?: string;
  purchasePrice: number;
  salePrice: number;
  brand: string;
  categoryId: string;
  categoryName: string;
}

// ── Customer ──
export interface CustomerCreate {
  firstName: string;
  middleName?: string;
  firstSurname: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  personTypeId: string;
  email?: string;
  telephoneNumber?: string;
}
export interface CustomerUpdate extends CustomerCreate {
  active?: boolean;
}
export interface CustomerResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  firstName: string;
  middleName?: string;
  firstSurname: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  documentIdName: string;
  personTypeId: string;
  personTypeName: string;
  email?: string;
  telephoneNumber?: string;
}

// ── Provider ──
export interface ProviderCreate {
  firstName: string;
  middleName?: string;
  firstSurname?: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  personTypeId: string;
  email: string;
  telephoneNumber: string;
  address: string;
}
export interface ProviderUpdate extends ProviderCreate {
  active?: boolean;
}
export interface ProviderResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  firstName: string;
  middleName?: string;
  firstSurname?: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  documentIdName: string;
  personTypeId: string;
  personTypeName: string;
  email: string;
  telephoneNumber: string;
  address: string;
}

// ── User ──
export interface UserCreate {
  firstName: string;
  middleName?: string;
  firstSurname: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  username: string;
  password: string;
  roleId: string;
}
export interface UserUpdate extends UserCreate {
  active?: boolean;
}
export interface UserResponse {
  id: string;
  active?: boolean;
  createdAt?: string;
  updatedAt?: string;
  firstName: string;
  middleName?: string;
  firstSurname: string;
  secondSurname?: string;
  documentIdNumber: string;
  docTypeId: string;
  documentIdName: string;
  username: string;
  roleId: string;
  roleName: string;
}

// ── Inventory ──
export interface InventoryUpdate {
  stock: number;
  minStock: number;
  location?: string;
}
export interface InventoryResponse {
  productSku: string;
  productName: string;
  storageId: number;
  storageName: string;
  stock: number;
  minStock: number;
  location?: string;
  lastReorderDate?: string;
  updatedAt?: string;
}

// ── InventoryMovement ──
export interface InventoryMovementResponse {
  id: string;
  productSku: string;
  productName: string;
  storageId: number;
  storageName: string;
  movementType: string;
  quantity: number;
  balanceAfter: number;
  referenceId?: string;
  userId: string;
  username: string;
  createdAt?: string;
  notes?: string;
}

// ── Purchase ──
export interface PurchaseDetailCreate {
  purchaseId?: string;
  productSku: string;
  storageId: number;
  quantity: number;
  unitCost: number;
  taxAmount: number;
  subtotal: number;
}
export interface PurchaseDetailResponse {
  id: string;
  purchaseId: string;
  productSku: string;
  productName: string;
  storageId: number;
  storageName: string;
  quantity: number;
  unitCost: number;
  taxAmount: number;
  subtotal: number;
}
export interface PurchaseCreate {
  providerInvoiceNumber: string;
  providerId: string;
  userId: string;
  status: string;
  details: PurchaseDetailCreate[];
}
export interface PurchaseUpdate {
  status: string;
}
export interface PurchaseResponse {
  id: string;
  providerInvoiceNumber: string;
  providerId: string;
  providerName: string;
  userId: string;
  username: string;
  totalAmount: number;
  status: string;
  createdAt?: string;
  details: PurchaseDetailResponse[];
}

// ── Sale ──
export interface SalesDetailCreate {
  saleId?: string;
  productSku: string;
  storageId: number;
  quantity: number;
  unitPrice: number;
  taxAmount: number;
  subtotal: number;
  discount?: number;
}
export interface SalesDetailResponse {
  id: string;
  saleId: string;
  productSku: string;
  productName: string;
  storageId: number;
  storageName: string;
  quantity: number;
  unitPrice: number;
  taxAmount: number;
  subtotal: number;
  discount?: number;
}
export interface SaleCreate {
  invoiceNumber: string;
  customerId: string;
  userId: string;
  totalDiscount?: number;
  status: string;
  details: SalesDetailCreate[];
}
export interface SaleUpdate {
  status: string;
}
export interface SaleResponse {
  id: string;
  invoiceNumber: string;
  customerId: string;
  customerName: string;
  userId: string;
  username: string;
  totalAmount: number;
  totalDiscount?: number;
  status: string;
  createdAt?: string;
  details: SalesDetailResponse[];
}

// ── Transfer ──
export interface TransferDetailCreate {
  transferId?: string;
  productSku: string;
  quantity: number;
}
export interface TransferDetailResponse {
  id: string;
  transferId: string;
  productSku: string;
  productName: string;
  quantity: number;
}
export interface TransferCreate {
  originStorageId: number;
  destinationStorageId: number;
  userId: string;
  status: string;
  details: TransferDetailCreate[];
}
export interface TransferResponse {
  id: string;
  originStorageId: number;
  originStorageName: string;
  destinationStorageId: number;
  destinationStorageName: string;
  userId: string;
  username: string;
  status?: string;
  createdAt?: string;
  details: TransferDetailResponse[];
}

// ── Reports ──
export interface PaginatedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface TopSoldProduct {
  sku: string;
  productName: string;
  brand: string;
  categoryName: string;
  totalQuantitySold: number;
  totalSalesValue: number;
}
export interface TopPurchasedProduct {
  sku: string;
  productName: string;
  brand: string;
  categoryName: string;
  totalQuantityPurchased: number;
  totalPurchaseValue: number;
}
export interface LowRotationProduct {
  sku: string;
  productName: string;
  brand: string;
  categoryName: string;
  totalQuantitySold: number;
  lastSaleDate?: string;
}
export interface LowStockProduct {
  productSku: string;
  productName: string;
  storageId: number;
  storageName: string;
  stock: number;
  minStock: number;
}
export interface ProductSalesValue {
  sku: string;
  productName: string;
  brand: string;
  categoryName: string;
  totalSalesValue: number;
}
export interface ProductInventoryValue {
  productSku: string;
  productName: string;
  brand: string;
  categoryName: string;
  storageId: number;
  storageName: string;
  stock: number;
  unitPrice: number;
  totalValue: number;
}
export interface TopCustomer {
  customerId: string;
  fullName: string;
  documentIdNumber: string;
  totalSalesCount: number;
  totalAmount: number;
}
export interface TopSeller {
  userId: string;
  fullName: string;
  username: string;
  totalSalesCount: number;
  totalAmount: number;
}
export interface CustomerProductDetail {
  productSku: string;
  productName: string;
  totalQuantity: number;
  totalAmount: number;
}
export interface SellerProductDetail {
  productSku: string;
  productName: string;
  totalQuantity: number;
  totalAmount: number;
}
