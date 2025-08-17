using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using FluentValidation;

namespace DemoEventi.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUserDto> _validator;
    private readonly IInterestRepository _interestRepository;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateUserDto> validator, IInterestRepository interestRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _interestRepository = interestRepository;
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto dto)
    {
        try
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<UserDto>.ValidationFailure(errors);
            }

            var user = _mapper.Map<User>(dto);
            user.DataOraCreazione = DateTime.UtcNow;

            // Handle interests if provided
            if (dto.InterestIds != null && dto.InterestIds.Any())
            {
                var interests = await _interestRepository.GetByIdsAsync(dto.InterestIds);
                user.Interests = interests.ToList();
            }

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error creating user: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        try
        {
            var users = await _unitOfWork.Users
                .GetAllAsync(includeProperties: nameof(User.Interests));

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, CreateUserDto dto)
    {
        try
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<UserDto>.ValidationFailure(errors);
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            // Update properties
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            existingUser.DataOraModifica = DateTime.UtcNow;

            // Handle interests update
            if (dto.InterestIds != null)
            {
                var interests = await _interestRepository.GetByIdsAsync(dto.InterestIds);
                existingUser.Interests = interests.ToList();
            }
            else
            {
                existingUser.Interests.Clear();
            }

            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(existingUser);
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Error updating user: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting user: {ex.Message}");
        }
    }
}
