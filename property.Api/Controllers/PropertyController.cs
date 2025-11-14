using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using property.Application.DTOs;
using property.Application.Interfaces;
using property.Domain.Entities;

namespace property.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize] 

public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var property = await _propertyService.GetAll();
        return Ok(property);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var property = await _propertyService.GetById(id);
        if (property == null)
            return NotFound();

        return Ok(property);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Property property, IFormFile? image)
    {
        UploadFileDto? dto = null;

        if (image != null)
        {
            dto = new UploadFileDto
            {
                FileName = image.FileName,
                FileStream = image.OpenReadStream()
            };
        }

        var created = await _propertyService.Create(property, dto);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Property property)
    {
        if (id != property.Id)
            return BadRequest("El ID no coincide.");

        var updated = await _propertyService.Update(property);
        return Ok(updated);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _propertyService.Delete(id);
        return NoContent();
    }
    
}