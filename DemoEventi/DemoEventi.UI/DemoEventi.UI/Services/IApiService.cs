using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.UI.Services;

public interface IApiService
{
    Task<Result<IEnumerable<UserDto>>> GetUsersAsync();
    Task<Result<UserDto>> GetUserAsync(Guid id);
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto user);
    Task<Result<UserDto>> UpdateUserAsync(Guid id, CreateUserDto user);
    Task<Result> DeleteUserAsync(Guid id);

    Task<Result<IEnumerable<EventDto>>> GetEventsAsync();
    Task<Result<EventDto>> GetEventAsync(Guid id);
    Task<Result<EventDto>> CreateEventAsync(CreateEventDto eventDto);
    Task<Result<EventDto>> UpdateEventAsync(Guid id, CreateEventDto eventDto);
    Task<Result> DeleteEventAsync(Guid id);

    Task<Result<IEnumerable<InterestDto>>> GetInterestsAsync();
}
