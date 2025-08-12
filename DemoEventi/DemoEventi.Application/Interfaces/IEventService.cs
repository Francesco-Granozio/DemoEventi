using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Interfaces;

public interface IEventService
{
    Task<Result<EventDto>> CreateAsync(CreateEventDto dto);
    Task<Result<IEnumerable<EventDto>>> GetAllAsync();
    Task<Result<EventDto>> GetByIdAsync(Guid id);
    Task<Result<EventDto>> UpdateAsync(Guid id, CreateEventDto dto);
    Task<Result> DeleteAsync(Guid id);
    Task<Result> AddParticipantsAsync(Guid eventId, IEnumerable<Guid> userIds);
}