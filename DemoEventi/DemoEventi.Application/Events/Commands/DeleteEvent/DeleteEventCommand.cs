using MediatR;
using DemoEventi.Application.Common;

namespace DemoEventi.Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}
