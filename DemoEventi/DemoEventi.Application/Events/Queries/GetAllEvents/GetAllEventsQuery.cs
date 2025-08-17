using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Events.Queries.GetAllEvents;

public record GetAllEventsQuery : IRequest<Result<IEnumerable<EventDto>>>
{
}
