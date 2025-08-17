using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using MediatR;

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
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.CreateUserDto.FirstName,
                LastName = request.CreateUserDto.LastName,
                DataOraCreazione = DateTime.UtcNow
            };

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
}
