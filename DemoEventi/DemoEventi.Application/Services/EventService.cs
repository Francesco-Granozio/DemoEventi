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

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EventDto> CreateAsync(CreateEventDto dto)
    {
        var ev = _mapper.Map<Event>(dto);
        ev.DataOraCreazione = DateTime.UtcNow;

        // Usa la proprietà Events invece di Repository<Event>()
        await _unitOfWork.Events.AddAsync(ev);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<EventDto>(ev);
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {
        // Qui includi i partecipanti con l’overload GetAllAsync
        var events = await _unitOfWork.Events
            .GetAllAsync(
                includeProperties: nameof(Event.Participants));

        return _mapper.Map<IEnumerable<EventDto>>(events);
    }
}
