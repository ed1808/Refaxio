using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize(Roles = "Admin,Director")]
[ApiController]
[Route("api/[controller]")]
public class ReportController(IReportService reportService) : ControllerBase
{
    [HttpGet("top-sold-products")]
    public async Task<IActionResult> GetTopSoldProducts(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetTopSoldProductsAsync(startDate, endDate, page, pageSize);
        return Ok(result);
    }

    [HttpGet("top-purchased-products")]
    public async Task<IActionResult> GetTopPurchasedProducts(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetTopPurchasedProductsAsync(startDate, endDate, page, pageSize);
        return Ok(result);
    }

    [HttpGet("low-rotation-products")]
    public async Task<IActionResult> GetLowRotationProducts(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetLowRotationProductsAsync(startDate, endDate, page, pageSize);
        return Ok(result);
    }

    [HttpGet("low-stock-products")]
    public async Task<IActionResult> GetLowStockProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await reportService.GetLowStockProductsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("products-by-sales-value")]
    public async Task<IActionResult> GetProductsBySalesValue(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetProductsBySalesValueAsync(startDate, endDate, page, pageSize);
        return Ok(result);
    }

    [HttpGet("products-by-inventory-value")]
    public async Task<IActionResult> GetProductsByInventoryValue(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await reportService.GetProductsByInventoryValueAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopCustomers(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? customerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetTopCustomersAsync(startDate, endDate, customerId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("top-sellers")]
    public async Task<IActionResult> GetTopSellers(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetTopSellersAsync(startDate, endDate, userId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("customers/{customerId:guid}/detail")]
    public async Task<IActionResult> GetCustomerDetail(
        Guid customerId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetCustomerDetailAsync(customerId, startDate, endDate, page, pageSize);
        return Ok(result);
    }

    [HttpGet("sellers/{userId:guid}/detail")]
    public async Task<IActionResult> GetSellerDetail(
        Guid userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            return BadRequest("startDate must be less than or equal to endDate.");

        var result = await reportService.GetSellerDetailAsync(userId, startDate, endDate, page, pageSize);
        return Ok(result);
    }
}
