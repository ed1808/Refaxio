using Backend.Dtos.PurchaseDetail;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PurchaseDetailController(IPurchaseDetailService purchaseDetailService) : ControllerBase
{
    [HttpGet("purchase/{purchaseId:guid}")]
    public async Task<IActionResult> GetAllByPurchaseId(Guid purchaseId)
    {
        var details = await purchaseDetailService.GetAllByPurchaseIdAsync(purchaseId);
        return Ok(details);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var detail = await purchaseDetailService.GetByIdAsync(id);
        if (detail is null)
            return NotFound();
        return Ok(detail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseDetailCreateDto dto)
    {
        var created = await purchaseDetailService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
