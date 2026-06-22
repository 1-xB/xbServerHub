using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Persistence.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<UserDto?> GetCurrentUser(string? userId)
    {
        if (userId == null)
        {
            return null;
        }
        
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }
        
        return new UserDto
        {
            UserName = user.UserName,
            Email = user.Email
        };
    }
}