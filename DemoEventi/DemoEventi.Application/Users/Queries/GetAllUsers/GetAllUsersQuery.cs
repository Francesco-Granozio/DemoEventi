using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
{
}
