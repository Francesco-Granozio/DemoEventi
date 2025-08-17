using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<Result<UserDto>>
{
    public Guid Id { get; init; }
    public required UpdateUserDto UpdateUserDto { get; init; }
}
