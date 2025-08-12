using AutoMapper;
using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;

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
                Name = request.Name,
                Location = request.Location,
                StartDate = request.StartDate,
                DataOraCreazione = DateTime.UtcNow
            };

            await _unitOfWork.Events.AddAsync(ev);
            await _unitOfWork.CommitAsync();

            var eventDto = _mapper.Map<EventDto>(ev);
            return Result<EventDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error creating event: {ex.Message}");
        }
    }
}
