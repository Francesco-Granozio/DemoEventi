using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<UserDto>>
{
    public required CreateUserDto CreateUserDto { get; init; }
}
