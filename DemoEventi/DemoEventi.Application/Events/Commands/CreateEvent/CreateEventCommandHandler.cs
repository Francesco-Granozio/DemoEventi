using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<EventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<EventDto>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Name = request.CreateEventDto.Name,
                Location = request.CreateEventDto.Location,
                StartDate = request.CreateEventDto.StartDate,
                DataOraCreazione = DateTime.UtcNow
            };

            await _unitOfWork.Events.AddAsync(ev);
            await _unitOfWork.SaveChangesAsync();

            var eventDto = _mapper.Map<EventDto>(ev);
            return Result<EventDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error creating event: {ex.Message}");
        }
    }
}
