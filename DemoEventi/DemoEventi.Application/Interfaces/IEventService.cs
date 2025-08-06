using DemoEventi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Interfaces;
public interface IEventService
{
    Task<EventDto> CreateAsync(CreateEventDto dto);
    Task<IEnumerable<EventDto>> GetAllAsync();
    // ulteriori metodi come GetById, Update, Delete...
}