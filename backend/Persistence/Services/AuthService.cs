using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.Identity;

namespace Persistence.Services;

public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthService> _logger) : IAuthService
{
    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            _logger.LogWarning("Login attempt failed: Email {Email} does not exist.", loginDto.Email);
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            loginDto.Password,
            isPersistent: true,
            lockoutOnFailure: true);
        
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked: {Email}", loginDto.Email);
            return new LoginResponseDto { IsSuccess = false, Message = "Account is temporarily locked. Please try again later." };
        }
        
        if (!result.Succeeded)
        {
            _logger.LogWarning("Invalid password attempt for user: {Email}", loginDto.Email);
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }
        _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
        return new LoginResponseDto { IsSuccess = true, Message = "Login successful" };
    }
}