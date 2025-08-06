using AutoMapper;
using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.DataOraCreazione = DateTime.UtcNow;

        // Usa la proprietà Users
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _unitOfWork.Users
            .GetAllAsync(includeProperties: nameof(User.Interests));

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}
