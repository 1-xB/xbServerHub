using System.Text.Encodings.Web;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.Identity;

namespace Persistence.Services;

public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthService> logger) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            logger.LogWarning("Login attempt failed: Email {Email} does not exist.", loginDto.Email);
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            loginDto.Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            return new LoginResponseDto { IsSuccess = false, RequiresTwoFactor = true,Message = "Two-factor authentication is required."};
        }
        
        if (result.IsLockedOut)
        {
            logger.LogWarning("User account locked: {Email}", loginDto.Email);
            return new LoginResponseDto { IsSuccess = false, Message = "Account is temporarily locked. Please try again later." };
        }
        
        if (!result.Succeeded)
        {
            logger.LogWarning("Invalid password attempt for user: {Email}", loginDto.Email);
            return new LoginResponseDto{IsSuccess = false, Message = "Incorrect email or password"};
        }
        logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
        return new LoginResponseDto { IsSuccess = true, Message = "Login successful" };
    }

    public async Task<LoginResponseDto> LoginWithAuthenticatorAsync(LoginWithAuthenticatorDto loginWithAuthenticatorDto)
    {
        var sanitizedCode = loginWithAuthenticatorDto.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(sanitizedCode, true, loginWithAuthenticatorDto.RememberDevice);
        if (result.Succeeded)
        {
            logger.LogInformation("User logged in with authenticator successfully.");
            return new()
            {
                IsSuccess = true,
                Message = "Login successful"
            };

        }
        
        if (result.IsLockedOut)
        {
            return new LoginResponseDto { IsSuccess = false, Message = "Account is temporarily locked. Please try again later." };
        }
        
        logger.LogWarning("Invalid authenticator code attempt.");
        return new()
        {
            IsSuccess = false,
            Message = "Invalid authenticator code"
        };
    }

    public async Task<DisableAuthenticatorResponseDto> DisableAuthenticatorAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        
        var disableResult = await userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disableResult.Succeeded)
        {
            logger.LogWarning("Failed to disable 2FA for user {UserId}: {Errors}", userId,
                string.Join(", ", disableResult.Errors.Select(e => e.Description)));
            return new()
            {
                IsSuccess = false,
                Message = "Failed to disable 2FA"
            };
        }
        
        logger.LogInformation("2FA disabled successfully for user: {UserId}", userId);
        return new()
        {
            IsSuccess = true,
            Message = "2FA disabled successfully"
        };
    }

    public async Task<EnableAuthenticatorResponseDto> EnableAuthenticatorAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        
        var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        }

        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            return new()
            {
                IsSuccess = false,
                Message = "Failed to generate authenticator key."
            };
        }
        
        var email = user.Email;
        var issuer = "xbServerHub";
        
        var encodedIssuer = UrlEncoder.Default.Encode(issuer);
        var encodedEmail = UrlEncoder.Default.Encode(email!);
        var authenticatorUri = $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={unformattedKey}&issuer={encodedIssuer}&digits=6";

        return new EnableAuthenticatorResponseDto
        {
            IsSuccess = true,
            Message = "Authenticator key generated successfully",
            SharedKey = unformattedKey,
            AuthenticatorUri = authenticatorUri
        };
    }

    public async Task<VerifyAuthenticatorResponseDto> VerifyAuthenticatorAsync(string userId, string code)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        var sanitizedCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);
        var isValid = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, sanitizedCode);

        if (!isValid)
        {
            logger.LogWarning("Invalid authenticator code for user: {UserId}", userId);
            return new()
            {
                IsSuccess = false,
                Message = "Invalid authenticator code"
            };
        }

        var enableResult = await userManager.SetTwoFactorEnabledAsync(user, true);
        if (!enableResult.Succeeded)
        {
            logger.LogWarning("Failed to enable 2FA for user {UserId}: {Errors}", userId,
                string.Join(", ", enableResult.Errors.Select(e => e.Description)));
            return new()
            {
                IsSuccess = false,
                Message = "Failed to enable 2FA"
            };
        }
        
        logger.LogInformation("2FA enabled successfully for user: {UserId}", userId);
        return new()
        {
            IsSuccess = true,
            Message = "2FA enabled successfully"
        };
    }

    public async Task<LogoutResponseDto> LogoutAsync(string userId)
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User {UserId} logged out successfully.", userId);
        return new()
        {
            IsSuccess = true,
            Message = "User successfully logged"
        };
    }
}