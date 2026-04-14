using Backend.Dtos.Sale;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SaleController(ISaleService saleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await saleService.GetAllAsync();
        return Ok(sales);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await saleService.GetByIdAsync(id);
        if (sale is null)
            return NotFound();
        return Ok(sale);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId)
    {
        var sales = await saleService.GetByCustomerIdAsync(customerId);
        return Ok(sales);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");
        var sales = await saleService.GetByStatusAsync(status);
        return Ok(sales);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaleCreateDto dto)
    {
        var created = await saleService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaleUpdateDto dto)
    {
        var updated = await saleService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await saleService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
