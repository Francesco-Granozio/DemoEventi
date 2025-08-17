using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
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
}
