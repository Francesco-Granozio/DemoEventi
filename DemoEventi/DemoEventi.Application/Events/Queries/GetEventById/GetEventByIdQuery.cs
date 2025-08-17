using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Events.Queries.GetEventById;

public record GetEventByIdQuery : IRequest<Result<EventDto>>
{
    public Guid Id { get; init; }
}
