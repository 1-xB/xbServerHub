namespace Application.DTOs.Auth;

public class LogoutResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } =  string.Empty;
}