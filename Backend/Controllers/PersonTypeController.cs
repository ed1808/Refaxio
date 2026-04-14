using Backend.Dtos.PersonType;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonTypeController(IPersonTypeService personTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var personTypes = await personTypeService.GetAllAsync();
        return Ok(personTypes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var personType = await personTypeService.GetByIdAsync(id);
        if (personType is null)
            return NotFound();
        return Ok(personType);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonTypeCreateDto dto)
    {
        var created = await personTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonTypeUpdateDto dto)
    {
        var updated = await personTypeService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await personTypeService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
