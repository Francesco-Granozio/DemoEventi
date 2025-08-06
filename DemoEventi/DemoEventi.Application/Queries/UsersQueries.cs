using DemoEventi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Queries;

public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;
