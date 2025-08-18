using DemoEventi.Application.DTOs;
using System.Text.Json;

namespace DemoEventi.UI.Services;

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;
    private UserDto? _currentUser;
    private const string USER_KEY = "current_user";

    public AuthService(IApiService apiService)
    {
        _apiService = apiService;
        _ = LoadCurrentUserAsync(); // Load user in background
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var result = await _apiService.LoginAsync(loginDto);
            if (result.IsSuccess && result.User != null)
            {
                await SetCurrentUserAsync(result.User);
            }
            return result;
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Error = $"Login failed: {ex.Message}"
            };
        }
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var result = await _apiService.RegisterAsync(registerDto);
            if (result.IsSuccess && result.User != null)
            {
                await SetCurrentUserAsync(result.User);
            }
            return result;
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                IsSuccess = false,
                Error = $"Registration failed: {ex.Message}"
            };
        }
    }

    public async Task LogoutAsync()
    {
        await ClearCurrentUserAsync();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        return await LoadCurrentUserAsync();
    }

    public async Task<Guid> GetCurrentUserIdAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.Id ?? Guid.Empty;
    }

    public async Task SetCurrentUserAsync(UserDto user)
    {
        _currentUser = user;
        await SaveCurrentUserAsync(user);
    }

    public async Task ClearCurrentUserAsync()
    {
        _currentUser = null;
        SecureStorage.Remove(USER_KEY);
        await Task.CompletedTask;
    }

    private async Task<UserDto?> LoadCurrentUserAsync()
    {
        try
        {
            var userJson = await SecureStorage.GetAsync(USER_KEY);
            if (!string.IsNullOrEmpty(userJson))
            {
                _currentUser = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return _currentUser;
            }
        }
        catch (Exception ex)
        {
            // Log error but don't throw - just return null
            System.Diagnostics.Debug.WriteLine($"Error loading current user: {ex.Message}");
        }

        return null;
    }

    private async Task SaveCurrentUserAsync(UserDto user)
    {
        try
        {
            var userJson = JsonSerializer.Serialize(user, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await SecureStorage.SetAsync(USER_KEY, userJson);
        }
        catch (Exception ex)
        {
            // Log error but don't throw
            System.Diagnostics.Debug.WriteLine($"Error saving current user: {ex.Message}");
        }
    }
}
