namespace Application.DTOs.Auth;

public class DisableAuthenticatorResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}