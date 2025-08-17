using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using MediatR;

namespace DemoEventi.Application.Interests.Queries.GetAllInterests;

public record GetAllInterestsQuery : IRequest<Result<IEnumerable<InterestDto>>>
{
}
