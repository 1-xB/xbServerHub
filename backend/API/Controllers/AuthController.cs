using System.Security.Claims;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController(IAuthService authService)
    : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);
        if (result.RequiresTwoFactor)
        {
            return Ok(result);
        }
        if (!result.IsSuccess) return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost("login-with-authenticator")]
    public async Task<IActionResult> LoginWithAuthenticator(LoginWithAuthenticatorDto loginWithAuthenticatorDto)
    {
        if (string.IsNullOrWhiteSpace(loginWithAuthenticatorDto.Code))
        {
            return BadRequest("Authenticator code is required");
        }
        var result = await authService.LoginWithAuthenticatorAsync(loginWithAuthenticatorDto);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("enable-authenticator")]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("User identifier not found");
        }

        var result = await authService.EnableAuthenticatorAsync(userId);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("disable-authenticator")]
    public async Task<IActionResult> DisableAuthenticator()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("User identifier not found");
        }

        var result = await authService.DisableAuthenticatorAsync(userId);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("verify-authenticator")]
    public async Task<IActionResult> VerifyAuthenticator(VerifyAuthenticatorDto verifyAuthenticatorDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("User identifier not found");
        }
        
        if (string.IsNullOrWhiteSpace(verifyAuthenticatorDto.Code))
        {
            return BadRequest("Authenticator code is required");
        }
        
        var result = await authService.VerifyAuthenticatorAsync(userId, verifyAuthenticatorDto.Code);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("User identifier not found");
        var result = await authService.LogoutAsync(userId);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return Ok(result);
    }
}