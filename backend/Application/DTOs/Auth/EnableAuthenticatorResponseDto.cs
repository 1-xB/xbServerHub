namespace Application.DTOs.Auth;

public class EnableAuthenticatorResponseDto
{
    public string SharedKey { get; set; } = string.Empty;
    public string AuthenticatorUri { get; set; } = string.Empty;
}