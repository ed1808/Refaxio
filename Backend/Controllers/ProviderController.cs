using Backend.Dtos.Provider;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize(Roles = "Admin,Director")]
[ApiController]
[Route("api/[controller]")]
public class ProviderController(IProviderService providerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var providers = await providerService.GetAllAsync();
        return Ok(providers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var provider = await providerService.GetByIdAsync(id);
        if (provider is null)
            return NotFound();
        return Ok(provider);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProviderCreateDto dto)
    {
        var created = await providerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProviderUpdateDto dto)
    {
        var updated = await providerService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await providerService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
