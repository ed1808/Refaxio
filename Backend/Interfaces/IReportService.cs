using Backend.Dtos.Report;

namespace Backend.Interfaces;

public interface IReportService
{
    Task<PaginatedResponseDto<TopSoldProductDto>> GetTopSoldProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize);

    Task<PaginatedResponseDto<TopPurchasedProductDto>> GetTopPurchasedProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize);

    Task<PaginatedResponseDto<LowRotationProductDto>> GetLowRotationProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize);

    Task<PaginatedResponseDto<LowStockProductDto>> GetLowStockProductsAsync(
        int page, int pageSize);

    Task<PaginatedResponseDto<ProductSalesValueDto>> GetProductsBySalesValueAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize);

    Task<PaginatedResponseDto<ProductInventoryValueDto>> GetProductsByInventoryValueAsync(
        int page, int pageSize);

    Task<PaginatedResponseDto<TopCustomerDto>> GetTopCustomersAsync(
        DateTime? startDate, DateTime? endDate, Guid? customerId, int page, int pageSize);

    Task<PaginatedResponseDto<TopSellerDto>> GetTopSellersAsync(
        DateTime? startDate, DateTime? endDate, Guid? userId, int page, int pageSize);

    Task<PaginatedResponseDto<CustomerProductDetailDto>> GetCustomerDetailAsync(
        Guid customerId, DateTime? startDate, DateTime? endDate, int page, int pageSize);

    Task<PaginatedResponseDto<SellerProductDetailDto>> GetSellerDetailAsync(
        Guid userId, DateTime? startDate, DateTime? endDate, int page, int pageSize);
}
