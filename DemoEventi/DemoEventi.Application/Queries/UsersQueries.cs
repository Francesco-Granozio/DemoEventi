using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Queries;

public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;
