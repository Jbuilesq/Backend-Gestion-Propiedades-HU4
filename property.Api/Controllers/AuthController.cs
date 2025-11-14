using System.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using property.Application.DTOs;
using property.Application.Interfaces;

namespace property.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var response = await _authService.RegisterAsync(registerDto);
            return Ok(response);
        }
        catch (SecurityException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Error interno del servidor ", details = e.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (SecurityException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, new {message = e.Message});
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenDto registerDto)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(registerDto);
            return Ok(result);
        }
        catch (SecurityException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RebokeToken(RevokeTokenDto revokeTokenDto)
    {
        try
        {
            var result = await _authService.RevokeTokenAsync(revokeTokenDto);
            return Ok(result);
        }
        
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Error interno del servidor", details = e.Message});
        }
    }
    
}