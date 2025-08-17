using DemoEventi.Application.Common;
using MediatR;

namespace DemoEventi.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}
