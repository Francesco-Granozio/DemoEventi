using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Events.Commands.UpdateEvent;

public record UpdateEventCommand : IRequest<Result<EventDto>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public IEnumerable<Guid> ParticipantIds { get; init; } = new List<Guid>();
}
