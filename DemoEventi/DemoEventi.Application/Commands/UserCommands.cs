using DemoEventi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Commands;

public record CreateUserCommand(CreateUserDto User) : IRequest<UserDto>;
