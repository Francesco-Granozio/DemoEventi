using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Events.Queries.GetAllEvents;

public record GetAllEventsQuery : IRequest<Result<IEnumerable<EventDto>>>
{
}
