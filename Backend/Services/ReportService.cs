using Backend.Dtos.Report;
using Backend.Interfaces;

namespace Backend.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;

    public ReportService(IReportRepository repository)
    {
        _repository = repository;
    }

    public Task<PaginatedResponseDto<TopSoldProductDto>> GetTopSoldProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetTopSoldProductsAsync(startDate, endDate, page, pageSize);

    public Task<PaginatedResponseDto<TopPurchasedProductDto>> GetTopPurchasedProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetTopPurchasedProductsAsync(startDate, endDate, page, pageSize);

    public Task<PaginatedResponseDto<LowRotationProductDto>> GetLowRotationProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetLowRotationProductsAsync(startDate, endDate, page, pageSize);

    public Task<PaginatedResponseDto<LowStockProductDto>> GetLowStockProductsAsync(
        int page, int pageSize) =>
        _repository.GetLowStockProductsAsync(page, pageSize);

    public Task<PaginatedResponseDto<ProductSalesValueDto>> GetProductsBySalesValueAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetProductsBySalesValueAsync(startDate, endDate, page, pageSize);

    public Task<PaginatedResponseDto<ProductInventoryValueDto>> GetProductsByInventoryValueAsync(
        int page, int pageSize) =>
        _repository.GetProductsByInventoryValueAsync(page, pageSize);

    public Task<PaginatedResponseDto<TopCustomerDto>> GetTopCustomersAsync(
        DateTime? startDate, DateTime? endDate, Guid? customerId, int page, int pageSize) =>
        _repository.GetTopCustomersAsync(startDate, endDate, customerId, page, pageSize);

    public Task<PaginatedResponseDto<TopSellerDto>> GetTopSellersAsync(
        DateTime? startDate, DateTime? endDate, Guid? userId, int page, int pageSize) =>
        _repository.GetTopSellersAsync(startDate, endDate, userId, page, pageSize);

    public Task<PaginatedResponseDto<CustomerProductDetailDto>> GetCustomerDetailAsync(
        Guid customerId, DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetCustomerDetailAsync(customerId, startDate, endDate, page, pageSize);

    public Task<PaginatedResponseDto<SellerProductDetailDto>> GetSellerDetailAsync(
        Guid userId, DateTime? startDate, DateTime? endDate, int page, int pageSize) =>
        _repository.GetSellerDetailAsync(userId, startDate, endDate, page, pageSize);
}
