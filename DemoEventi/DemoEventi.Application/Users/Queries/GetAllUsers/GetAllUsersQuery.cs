using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
{
}
