using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Interfaces;

public interface IInterestService
{
    Task<Result<IEnumerable<InterestDto>>> GetAllAsync();
    Task<Result<InterestDto>> GetByIdAsync(Guid id);
}
