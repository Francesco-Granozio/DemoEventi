using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<UserDto>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public IEnumerable<Guid> InterestIds { get; init; } = new List<Guid>();
}
