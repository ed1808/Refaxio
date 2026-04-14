using Backend.Dtos.Inventory;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var inventory = await inventoryService.GetAllAsync();
        return Ok(inventory);
    }

    [HttpGet("{productSku}/{storageId:int}")]
    public async Task<IActionResult> GetByKey(string productSku, int storageId)
    {
        var item = await inventoryService.GetByKeyAsync(productSku, storageId);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpGet("product/{productSku}")]
    public async Task<IActionResult> GetByProductSku(string productSku)
    {
        if (string.IsNullOrWhiteSpace(productSku))
            return BadRequest("Product SKU is required.");
        var items = await inventoryService.GetByProductSkuAsync(productSku);
        return Ok(items);
    }

    [HttpGet("storage/{storageId:int}")]
    public async Task<IActionResult> GetByStorageId(int storageId)
    {
        var items = await inventoryService.GetByStorageIdAsync(storageId);
        return Ok(items);
    }

    [HttpPut("{productSku}/{storageId:int}")]
    public async Task<IActionResult> Update(string productSku, int storageId, [FromBody] InventoryUpdateDto dto)
    {
        var updated = await inventoryService.UpdateAsync(productSku, storageId, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }
}
