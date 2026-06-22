namespace Application.DTOs.Auth;

public class LoginWithAuthenticatorDto
{
    public string Code { get; set; } = string.Empty;
    public bool RememberDevice { get; set; } = false;
}