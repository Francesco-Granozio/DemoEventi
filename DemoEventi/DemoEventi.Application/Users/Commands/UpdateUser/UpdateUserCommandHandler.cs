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
            existingUser.DataOraModifica = DateTime.UtcNow;

            // TODO: Handle interests update
            // existingUser.Interests = await _unitOfWork.Interests.GetByIdsAsync(request.InterestIds);

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
