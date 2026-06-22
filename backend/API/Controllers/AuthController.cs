using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController(IAuthService authService)
    : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await authService.Login(loginDto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
}