using DemoEventi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<IEnumerable<UserDto>> GetAllAsync();
    // ulteriori metodi come GetById, Update, Delete...
}
