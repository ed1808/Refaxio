using Backend.Dtos.Report;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponseDto<TopSoldProductDto>> GetTopSoldProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var query = _context.SalesDetails
            .Include(d => d.sale)
            .Include(d => d.productSkuNavigation)
                .ThenInclude(p => p.category)
            .Where(d => d.sale.status == "COMPLETED")
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.sale.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(d => d.sale.createdAt < endDate.Value.AddDays(1));

        var grouped = query
            .GroupBy(d => new
            {
                d.productSku,
                d.productSkuNavigation.productName,
                d.productSkuNavigation.brand,
                CategoryName = d.productSkuNavigation.category.categoryName
            })
            .Select(g => new TopSoldProductDto
            {
                Sku = g.Key.productSku,
                ProductName = g.Key.productName,
                Brand = g.Key.brand,
                CategoryName = g.Key.CategoryName,
                TotalQuantitySold = g.Sum(x => x.quantity),
                TotalSalesValue = g.Sum(x => x.subtotal)
            })
            .OrderByDescending(x => x.TotalQuantitySold);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<TopSoldProductDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<TopPurchasedProductDto>> GetTopPurchasedProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var query = _context.PurchaseDetails
            .Include(d => d.purchase)
            .Include(d => d.productSkuNavigation)
                .ThenInclude(p => p.category)
            .Where(d => d.purchase.status == "RECEIVED")
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.purchase.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(d => d.purchase.createdAt < endDate.Value.AddDays(1));

        var grouped = query
            .GroupBy(d => new
            {
                d.productSku,
                d.productSkuNavigation.productName,
                d.productSkuNavigation.brand,
                CategoryName = d.productSkuNavigation.category.categoryName
            })
            .Select(g => new TopPurchasedProductDto
            {
                Sku = g.Key.productSku,
                ProductName = g.Key.productName,
                Brand = g.Key.brand,
                CategoryName = g.Key.CategoryName,
                TotalQuantityPurchased = g.Sum(x => x.quantity),
                TotalPurchaseValue = g.Sum(x => x.subtotal)
            })
            .OrderByDescending(x => x.TotalQuantityPurchased);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<TopPurchasedProductDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<LowRotationProductDto>> GetLowRotationProductsAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var salesQuery = _context.SalesDetails
            .Where(d => d.sale.status == "COMPLETED")
            .AsQueryable();

        if (startDate.HasValue)
            salesQuery = salesQuery.Where(d => d.sale.createdAt >= startDate.Value);
        if (endDate.HasValue)
            salesQuery = salesQuery.Where(d => d.sale.createdAt < endDate.Value.AddDays(1));

        var salesAgg = await salesQuery
            .GroupBy(d => d.productSku)
            .Select(g => new
            {
                ProductSku = g.Key,
                TotalQuantitySold = g.Sum(x => x.quantity),
                LastSaleDate = g.Max(x => x.sale.createdAt)
            })
            .ToListAsync();

        var salesDict = salesAgg.ToDictionary(s => s.ProductSku);

        var products = await _context.Products
            .Include(p => p.category)
            .Where(p => p.active == true)
            .ToListAsync();

        var grouped = products
            .Select(p =>
            {
                salesDict.TryGetValue(p.sku, out var s);
                return new LowRotationProductDto
                {
                    Sku = p.sku,
                    ProductName = p.productName,
                    Brand = p.brand,
                    CategoryName = p.category.categoryName,
                    TotalQuantitySold = s?.TotalQuantitySold ?? 0,
                    LastSaleDate = s?.LastSaleDate
                };
            })
            .OrderBy(x => x.TotalQuantitySold)
            .ThenBy(x => x.Sku)
            .ToList();

        var totalCount = grouped.Count;
        var items = grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedResponseDto<LowRotationProductDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<LowStockProductDto>> GetLowStockProductsAsync(
        int page, int pageSize)
    {
        var query = _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .Where(i => i.stock <= i.minStock)
            .Select(i => new LowStockProductDto
            {
                ProductSku = i.productSku,
                ProductName = i.productSkuNavigation.productName,
                StorageId = i.storageId,
                StorageName = i.storage.storageName,
                Stock = i.stock,
                MinStock = i.minStock
            })
            .OrderBy(x => x.Stock)
            .ThenBy(x => x.ProductSku);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<LowStockProductDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<ProductSalesValueDto>> GetProductsBySalesValueAsync(
        DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var query = _context.SalesDetails
            .Include(d => d.sale)
            .Include(d => d.productSkuNavigation)
                .ThenInclude(p => p.category)
            .Where(d => d.sale.status == "COMPLETED")
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.sale.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(d => d.sale.createdAt < endDate.Value.AddDays(1));

        var grouped = query
            .GroupBy(d => new
            {
                d.productSku,
                d.productSkuNavigation.productName,
                d.productSkuNavigation.brand,
                CategoryName = d.productSkuNavigation.category.categoryName
            })
            .Select(g => new ProductSalesValueDto
            {
                Sku = g.Key.productSku,
                ProductName = g.Key.productName,
                Brand = g.Key.brand,
                CategoryName = g.Key.CategoryName,
                TotalSalesValue = g.Sum(x => x.subtotal)
            })
            .OrderByDescending(x => x.TotalSalesValue);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<ProductSalesValueDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<ProductInventoryValueDto>> GetProductsByInventoryValueAsync(
        int page, int pageSize)
    {
        var query = _context.Inventories
            .Include(i => i.productSkuNavigation)
                .ThenInclude(p => p.category)
            .Include(i => i.storage)
            .Where(i => i.stock > 0)
            .Select(i => new ProductInventoryValueDto
            {
                ProductSku = i.productSku,
                ProductName = i.productSkuNavigation.productName,
                Brand = i.productSkuNavigation.brand,
                CategoryName = i.productSkuNavigation.category.categoryName,
                StorageId = i.storageId,
                StorageName = i.storage.storageName,
                Stock = i.stock,
                UnitPrice = i.productSkuNavigation.salePrice,
                TotalValue = i.stock * i.productSkuNavigation.salePrice
            })
            .OrderByDescending(x => x.TotalValue);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<ProductInventoryValueDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<TopCustomerDto>> GetTopCustomersAsync(
        DateTime? startDate, DateTime? endDate, Guid? customerId, int page, int pageSize)
    {
        var query = _context.Sales
            .Include(s => s.customer)
            .Where(s => s.status == "COMPLETED")
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.createdAt < endDate.Value.AddDays(1));
        if (customerId.HasValue)
            query = query.Where(s => s.customerId == customerId.Value);

        var grouped = query
            .GroupBy(s => new
            {
                s.customerId,
                s.customer.firstName,
                s.customer.middleName,
                s.customer.firstSurname,
                s.customer.secondSurname,
                s.customer.documentIdNumber
            })
            .Select(g => new TopCustomerDto
            {
                CustomerId = g.Key.customerId,
                FullName = g.Key.firstName
                    + (g.Key.middleName != null ? " " + g.Key.middleName : "")
                    + " " + g.Key.firstSurname
                    + (g.Key.secondSurname != null ? " " + g.Key.secondSurname : ""),
                DocumentIdNumber = g.Key.documentIdNumber,
                TotalSalesCount = g.Count(),
                TotalAmount = g.Sum(x => x.totalAmount)
            })
            .OrderByDescending(x => x.TotalAmount);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<TopCustomerDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<TopSellerDto>> GetTopSellersAsync(
        DateTime? startDate, DateTime? endDate, Guid? userId, int page, int pageSize)
    {
        var query = _context.Sales
            .Include(s => s.user)
            .Where(s => s.status == "COMPLETED")
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.createdAt < endDate.Value.AddDays(1));
        if (userId.HasValue)
            query = query.Where(s => s.userId == userId.Value);

        var grouped = query
            .GroupBy(s => new
            {
                s.userId,
                s.user.firstName,
                s.user.middleName,
                s.user.firstSurname,
                s.user.secondSurname,
                s.user.username
            })
            .Select(g => new TopSellerDto
            {
                UserId = g.Key.userId,
                FullName = g.Key.firstName
                    + (g.Key.middleName != null ? " " + g.Key.middleName : "")
                    + " " + g.Key.firstSurname
                    + (g.Key.secondSurname != null ? " " + g.Key.secondSurname : ""),
                Username = g.Key.username,
                TotalSalesCount = g.Count(),
                TotalAmount = g.Sum(x => x.totalAmount)
            })
            .OrderByDescending(x => x.TotalAmount);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<TopSellerDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<CustomerProductDetailDto>> GetCustomerDetailAsync(
        Guid customerId, DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var query = _context.SalesDetails
            .Include(d => d.sale)
            .Include(d => d.productSkuNavigation)
            .Where(d => d.sale.status == "COMPLETED" && d.sale.customerId == customerId)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.sale.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(d => d.sale.createdAt < endDate.Value.AddDays(1));

        var grouped = query
            .GroupBy(d => new
            {
                d.productSku,
                d.productSkuNavigation.productName
            })
            .Select(g => new CustomerProductDetailDto
            {
                ProductSku = g.Key.productSku,
                ProductName = g.Key.productName,
                TotalQuantity = g.Sum(x => x.quantity),
                TotalAmount = g.Sum(x => x.subtotal)
            })
            .OrderByDescending(x => x.TotalAmount);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<CustomerProductDetailDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponseDto<SellerProductDetailDto>> GetSellerDetailAsync(
        Guid userId, DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
        var query = _context.SalesDetails
            .Include(d => d.sale)
            .Include(d => d.productSkuNavigation)
            .Where(d => d.sale.status == "COMPLETED" && d.sale.userId == userId)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.sale.createdAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(d => d.sale.createdAt < endDate.Value.AddDays(1));

        var grouped = query
            .GroupBy(d => new
            {
                d.productSku,
                d.productSkuNavigation.productName
            })
            .Select(g => new SellerProductDetailDto
            {
                ProductSku = g.Key.productSku,
                ProductName = g.Key.productName,
                TotalQuantity = g.Sum(x => x.quantity),
                TotalAmount = g.Sum(x => x.subtotal)
            })
            .OrderByDescending(x => x.TotalAmount);

        var totalCount = await grouped.CountAsync();
        var items = await grouped
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<SellerProductDetailDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
