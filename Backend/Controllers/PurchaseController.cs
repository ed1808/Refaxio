using Backend.Dtos.Purchase;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PurchaseController(IPurchaseService purchaseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var purchases = await purchaseService.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchase = await purchaseService.GetByIdAsync(id);
        if (purchase is null)
            return NotFound();
        return Ok(purchase);
    }

    [HttpGet("provider/{providerId:guid}")]
    public async Task<IActionResult> GetByProviderId(Guid providerId)
    {
        var purchases = await purchaseService.GetByProviderIdAsync(providerId);
        return Ok(purchases);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");
        var purchases = await purchaseService.GetByStatusAsync(status);
        return Ok(purchases);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseCreateDto dto)
    {
        var created = await purchaseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PurchaseUpdateDto dto)
    {
        var updated = await purchaseService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await purchaseService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
