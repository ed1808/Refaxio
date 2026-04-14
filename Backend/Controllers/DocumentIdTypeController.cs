using Backend.Dtos.DocumentIdType;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentIdTypeController(IDocumentIdTypeService documentIdTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var documentIdTypes = await documentIdTypeService.GetAllAsync();
        return Ok(documentIdTypes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var documentIdType = await documentIdTypeService.GetByIdAsync(id);
        if (documentIdType is null)
            return NotFound();
        return Ok(documentIdType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DocumentIdTypeCreateDto dto)
    {
        var created = await documentIdTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DocumentIdTypeUpdateDto dto)
    {
        var updated = await documentIdTypeService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await documentIdTypeService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
