using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Interests.Queries.GetAllInterests;

public class GetAllInterestsQueryHandler : IRequestHandler<GetAllInterestsQuery, Result<IEnumerable<InterestDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllInterestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<InterestDto>>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var interests = await _unitOfWork.Interests.GetAllAsync();
            var interestDtos = _mapper.Map<IEnumerable<InterestDto>>(interests);
            return Result<IEnumerable<InterestDto>>.Success(interestDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<InterestDto>>.Failure($"Error retrieving interests: {ex.Message}");
        }
    }
}
