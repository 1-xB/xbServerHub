using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<LoginResponseDto> LoginWithAuthenticatorAsync(LoginWithAuthenticatorDto loginWithAuthenticatorDto);
    Task<EnableAuthenticatorResponseDto> EnableAuthenticatorAsync(string userId);
    Task<VerifyAuthenticatorResponseDto> VerifyAuthenticatorAsync(string userId, string code);
}