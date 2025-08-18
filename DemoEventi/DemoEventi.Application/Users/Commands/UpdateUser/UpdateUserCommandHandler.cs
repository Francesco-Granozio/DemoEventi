using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (existingUser == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            // Update properties
            existingUser.FirstName = request.UpdateUserDto.FirstName;
            existingUser.LastName = request.UpdateUserDto.LastName;
            existingUser.Email = request.UpdateUserDto.Email ?? existingUser.Email;
            existingUser.ProfileImageUrl = request.UpdateUserDto.ProfileImageUrl;
            existingUser.DataOraModifica = DateTime.UtcNow;

            // Handle interests update
            if (request.UpdateUserDto.InterestIds != null)
            {
                // Clear existing interests
                existingUser.Interests.Clear();
                
                // Add new interests
                foreach (var interestId in request.UpdateUserDto.InterestIds)
                {
                    var interest = await _unitOfWork.Interests.GetByIdAsync(interestId);
                    if (interest != null)
                    {
                        existingUser.Interests.Add(interest);
                    }
                }
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
}
