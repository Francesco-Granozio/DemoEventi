using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, Result<IEnumerable<EventDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<EventDto>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _unitOfWork.Events
                .GetAllAsync(includeProperties: nameof(Event.Participants));

            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return Result<IEnumerable<EventDto>>.Success(eventDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EventDto>>.Failure($"Error retrieving events: {ex.Message}");
        }
    }
}
