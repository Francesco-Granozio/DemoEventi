using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using System.Text;
using System.Text.Json;

namespace DemoEventi.UI.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string[] _fallbackUrls;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Fallback URLs to try if primary fails
        _fallbackUrls = new[]
        {
            "http://10.0.2.2:5163/", // Android emulator host (primary fallback)
            "http://192.168.1.4:5163/", // Current host IP
            "https://192.168.1.4:7042/",
            "http://localhost:5163/",
            "https://localhost:7042/"
        };
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/users");
            return await HandleResponseAsync<IEnumerable<UserDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> GetUserAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");
            return await HandleResponseAsync<UserDto>(response);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/users", content);
            return await HandleResponseAsync<UserDto>(response);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error creating user: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> UpdateUserAsync(Guid id, CreateUserDto user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/users/{id}", content);
            return await HandleResponseAsync<UserDto>(response);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error updating user: {ex.Message}");
        }
    }

    public async Task<Result> DeleteUserAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            return await HandleResponseAsync(response);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<EventDto>>> GetEventsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/events");
            return await HandleResponseAsync<IEnumerable<EventDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EventDto>>.Failure($"Error retrieving events: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> GetEventAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/events/{id}");
            return await HandleResponseAsync<EventDto>(response);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error retrieving event: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> CreateEventAsync(CreateEventDto eventDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(eventDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/events", content);
            return await HandleResponseAsync<EventDto>(response);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error creating event: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> UpdateEventAsync(Guid id, CreateEventDto eventDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(eventDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/events/{id}", content);
            return await HandleResponseAsync<EventDto>(response);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error updating event: {ex.Message}");
        }
    }

    public async Task<Result> DeleteEventAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/events/{id}");
            return await HandleResponseAsync(response);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting event: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<InterestDto>>> GetInterestsAsync()
    {
        return await TryMultipleUrlsAsync<IEnumerable<InterestDto>>("api/interests", async (client, url) =>
        {
            var response = await client.GetAsync(url);
            return await HandleResponseAsync<IEnumerable<InterestDto>>(response);
        });
    }

    private async Task<Result<T>> TryMultipleUrlsAsync<T>(string endpoint, Func<HttpClient, string, Task<Result<T>>> operation)
    {
        var errors = new List<string>();

        // Try primary URL first
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            using var client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
            {
                BaseAddress = _httpClient.BaseAddress,
                Timeout = TimeSpan.FromSeconds(8)
            };

            var result = await operation(client, endpoint);
            if (result.IsSuccess)
            {
                return result;
            }
            errors.Add($"Primary URL failed: {result.Error}");
        }
        catch (Exception ex)
        {
            errors.Add($"Primary URL exception: {ex.Message}");
        }

        // Try fallback URLs
        foreach (var fallbackUrl in _fallbackUrls)
        {
            try
            {
                using var client = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                {
                    BaseAddress = new Uri(fallbackUrl),
                    Timeout = TimeSpan.FromSeconds(5)
                };

                var result = await operation(client, endpoint);
                if (result.IsSuccess)
                {
                    // Update primary client for future requests
                    _httpClient.BaseAddress = new Uri(fallbackUrl);
                    return result;
                }
                errors.Add($"{fallbackUrl}: {result.Error}");
            }
            catch (Exception ex)
            {
                errors.Add($"{fallbackUrl}: {ex.Message}");
            }
        }

        var combinedError = string.Join("; ", errors);
        return Result<T>.Failure($"Could not connect to server. Tried multiple URLs. Errors: {combinedError}");
    }

    private async Task<Result<T>> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                return Result<T>.Success(result!);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error deserializing response: {ex.Message}");
            }
        }
        else
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, _jsonOptions);
                return Result<T>.Failure(errorResponse?.Message ?? $"HTTP {response.StatusCode}");
            }
            catch
            {
                return Result<T>.Failure($"HTTP {response.StatusCode}: {content}");
            }
        }
    }

    private async Task<Result> HandleResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, _jsonOptions);
                return Result.Failure(errorResponse?.Message ?? $"HTTP {response.StatusCode}");
            }
            catch
            {
                return Result.Failure($"HTTP {response.StatusCode}: {content}");
            }
        }
    }

    private class ErrorResponse
    {
        public string? Message { get; set; }
    }
}
