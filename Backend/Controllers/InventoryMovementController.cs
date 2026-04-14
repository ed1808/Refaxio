using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryMovementController(IInventoryMovementService inventoryMovementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movements = await inventoryMovementService.GetAllAsync();
        return Ok(movements);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movement = await inventoryMovementService.GetByIdAsync(id);
        if (movement is null)
            return NotFound();
        return Ok(movement);
    }

    [HttpGet("product/{productSku}")]
    public async Task<IActionResult> GetByProductSku(string productSku)
    {
        if (string.IsNullOrWhiteSpace(productSku))
            return BadRequest("Product SKU is required.");
        var movements = await inventoryMovementService.GetByProductSkuAsync(productSku);
        return Ok(movements);
    }

    [HttpGet("storage/{storageId:int}")]
    public async Task<IActionResult> GetByStorageId(int storageId)
    {
        var movements = await inventoryMovementService.GetByStorageIdAsync(storageId);
        return Ok(movements);
    }
}
