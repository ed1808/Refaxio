using Backend.Dtos.SalesDetail;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesDetailController(ISalesDetailService salesDetailService) : ControllerBase
{
    [HttpGet("sale/{saleId:guid}")]
    public async Task<IActionResult> GetAllBySaleId(Guid saleId)
    {
        var details = await salesDetailService.GetAllBySaleIdAsync(saleId);
        return Ok(details);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var detail = await salesDetailService.GetByIdAsync(id);
        if (detail is null)
            return NotFound();
        return Ok(detail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesDetailCreateDto dto)
    {
        var created = await salesDetailService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
