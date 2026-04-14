using Backend.Dtos.Storage;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController(IStorageService storageService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var storages = await storageService.GetAllAsync();
        return Ok(storages);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var storage = await storageService.GetByIdAsync(id);
        if (storage is null)
            return NotFound();
        return Ok(storage);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StorageCreateDto dto)
    {
        var created = await storageService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StorageUpdateDto dto)
    {
        var updated = await storageService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await storageService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
