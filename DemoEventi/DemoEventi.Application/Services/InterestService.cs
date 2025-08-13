using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using DemoEventi.Domain.Interfaces;

namespace DemoEventi.Application.Services;

public class InterestService : IInterestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InterestService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<InterestDto>>> GetAllAsync()
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

    public async Task<Result<InterestDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var interest = await _unitOfWork.Interests.GetByIdAsync(id);
            if (interest == null)
            {
                return Result<InterestDto>.Failure("Interest not found");
            }

            var interestDto = _mapper.Map<InterestDto>(interest);
            return Result<InterestDto>.Success(interestDto);
        }
        catch (Exception ex)
        {
            return Result<InterestDto>.Failure($"Error retrieving interest: {ex.Message}");
        }
    }
}
