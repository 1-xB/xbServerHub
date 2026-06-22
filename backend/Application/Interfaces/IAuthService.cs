using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    public Task<LoginResponseDto> Login(LoginDto loginDto);
}