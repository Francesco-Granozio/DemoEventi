using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public Guid Id { get; init; }
}
