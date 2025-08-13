using System.Text;
using System.Text.Json;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.UI.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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
        try
        {
            var response = await _httpClient.GetAsync("api/Interests");
            return await HandleResponseAsync<IEnumerable<InterestDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<InterestDto>>.Failure($"Error retrieving interests: {ex.Message}");
        }
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
