using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace DemoEventi.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            if (!string.IsNullOrEmpty(request.CreateUserDto.Email))
            {
                var existingUsers = await _unitOfWork.Users.GetAllAsync();
                if (existingUsers.Any(u => u.Email?.Equals(request.CreateUserDto.Email, StringComparison.OrdinalIgnoreCase) == true))
                {
                    return Result<UserDto>.Failure("User with this email already exists");
                }
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.CreateUserDto.FirstName,
                LastName = request.CreateUserDto.LastName,
                Email = request.CreateUserDto.Email,
                PasswordHash = !string.IsNullOrEmpty(request.CreateUserDto.Password) ? HashPassword(request.CreateUserDto.Password) : string.Empty,
                ProfileImageUrl = request.CreateUserDto.ProfileImageUrl,
                DataOraCreazione = DateTime.UtcNow
            };

            // Add interests if provided
            if (request.CreateUserDto.InterestIds != null && request.CreateUserDto.InterestIds.Any())
            {
                foreach (var interestId in request.CreateUserDto.InterestIds)
                {
                    var interest = await _unitOfWork.Interests.GetByIdAsync(interestId);
                    if (interest != null)
                    {
                        user.Interests.Add(interest);
                    }
                }
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

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "DemoEventi_Salt"));
        return Convert.ToBase64String(hashedBytes);
    }
}
