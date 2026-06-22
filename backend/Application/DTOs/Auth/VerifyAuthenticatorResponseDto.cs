namespace Application.DTOs.Auth;

public class VerifyAuthenticatorResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}