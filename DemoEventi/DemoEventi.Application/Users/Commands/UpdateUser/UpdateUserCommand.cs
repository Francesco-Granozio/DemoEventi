using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<Result<UserDto>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public IEnumerable<Guid> InterestIds { get; init; } = new List<Guid>();
}
