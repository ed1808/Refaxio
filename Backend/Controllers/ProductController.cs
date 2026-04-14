using Backend.Dtos.Product;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize(Roles = "Admin,Director")]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{sku}")]
    public async Task<IActionResult> GetById(string sku)
    {
        var product = await productService.GetByIdAsync(sku);
        if (product is null)
            return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var created = await productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { sku = created.Sku }, created);
    }

    [HttpPut("{sku}")]
    public async Task<IActionResult> Update(string sku, [FromBody] ProductUpdateDto dto)
    {
        var updated = await productService.UpdateAsync(sku, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{sku}")]
    public async Task<IActionResult> Delete(string sku)
    {
        var deleted = await productService.DeleteAsync(sku);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
