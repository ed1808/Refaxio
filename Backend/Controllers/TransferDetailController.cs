using Backend.Dtos.TransferDetail;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransferDetailController(ITransferDetailService transferDetailService) : ControllerBase
{
    [HttpGet("transfer/{transferId:guid}")]
    public async Task<IActionResult> GetAllByTransferId(Guid transferId)
    {
        var details = await transferDetailService.GetAllByTransferIdAsync(transferId);
        return Ok(details);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var detail = await transferDetailService.GetByIdAsync(id);
        if (detail is null)
            return NotFound();
        return Ok(detail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransferDetailCreateDto dto)
    {
        var created = await transferDetailService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
