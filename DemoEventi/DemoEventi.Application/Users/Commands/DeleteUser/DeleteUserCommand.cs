using MediatR;
using DemoEventi.Application.Common;

namespace DemoEventi.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}
