using AutoMapper;
using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Interfaces;

namespace DemoEventi.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Result<EventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<EventDto>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(request.Id);
            if (ev == null)
            {
                return Result<EventDto>.Failure("Event not found");
            }

            var eventDto = _mapper.Map<EventDto>(ev);
            return Result<EventDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error retrieving event: {ex.Message}");
        }
    }
}
