using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using property.Application.Interfaces;

namespace property.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 

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
    
    
}