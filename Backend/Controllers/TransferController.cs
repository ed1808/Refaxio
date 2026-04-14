using Backend.Dtos.Transfer;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferController(ITransferService transferService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transfers = await transferService.GetAllAsync();
        return Ok(transfers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var transfer = await transferService.GetByIdAsync(id);
        if (transfer is null)
            return NotFound();
        return Ok(transfer);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");
        var transfers = await transferService.GetByStatusAsync(status);
        return Ok(transfers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransferCreateDto dto)
    {
        if (dto.OriginStorageId == dto.DestinationStorageId)
            return BadRequest("Origin and destination storage must be different.");
        var created = await transferService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await transferService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
