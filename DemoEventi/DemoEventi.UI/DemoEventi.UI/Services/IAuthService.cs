using DemoEventi.Application.DTOs;

namespace DemoEventi.UI.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserDto?> GetCurrentUserAsync();
    Task<Guid> GetCurrentUserIdAsync();
    Task SetCurrentUserAsync(UserDto user);
    Task ClearCurrentUserAsync();
}
