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
            System.Diagnostics.Debug.WriteLine("InterestService.GetAllAsync: Starting...");
            var interests = await _unitOfWork.Interests.GetAllAsync();
            System.Diagnostics.Debug.WriteLine($"InterestService.GetAllAsync: Retrieved {interests?.Count() ?? 0} interests");
            var interestDtos = _mapper.Map<IEnumerable<InterestDto>>(interests);
            System.Diagnostics.Debug.WriteLine($"InterestService.GetAllAsync: Mapped to {interestDtos?.Count() ?? 0} DTOs");
            return Result<IEnumerable<InterestDto>>.Success(interestDtos);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InterestService.GetAllAsync: Exception: {ex}");
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
