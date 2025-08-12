using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;

namespace DemoEventi.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> CreateAsync(CreateUserDto dto);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(Guid id);
    Task<Result<UserDto>> UpdateAsync(Guid id, CreateUserDto dto);
    Task<Result> DeleteAsync(Guid id);
}
