using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Persistence.Services;

public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            loginDto.Password,
            isPersistent: true,
            lockoutOnFailure: true);
        
        if (result.IsLockedOut)
        {
            return new LoginResponseDto { IsSuccess = false, Message = "Account is temporarily locked. Please try again later." };
        }
        
        if (!result.Succeeded)
        {
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }

        return new LoginResponseDto { IsSuccess = true, Message = "Login successful" };
    }
}