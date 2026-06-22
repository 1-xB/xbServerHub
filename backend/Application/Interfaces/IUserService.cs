using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<UserDto?> GetCurrentUser(string? userId);
}