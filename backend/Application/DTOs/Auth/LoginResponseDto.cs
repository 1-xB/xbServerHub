namespace Application.DTOs.Auth;

public class LoginResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}