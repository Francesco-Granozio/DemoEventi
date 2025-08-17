using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Events.Commands.CreateEvent;

public record CreateEventCommand : IRequest<Result<EventDto>>
{
    public required CreateEventDto CreateEventDto { get; init; }
}
