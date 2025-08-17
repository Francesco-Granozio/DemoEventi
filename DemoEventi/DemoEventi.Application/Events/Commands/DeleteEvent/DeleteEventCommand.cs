using DemoEventi.Application.Common;
using MediatR;

namespace DemoEventi.Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}
