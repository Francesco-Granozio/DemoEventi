using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Queries;

public record GetEventsQuery() : IRequest<IEnumerable<EventDto>>;
