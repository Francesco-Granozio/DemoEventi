using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Result<EventDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<EventDto>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(request.Id);
            if (existingEvent == null)
            {
                return Result<EventDto>.Failure("Event not found");
            }

            // Update properties
            existingEvent.Name = request.Name;
            existingEvent.Location = request.Location;
            existingEvent.StartDate = request.StartDate;
            existingEvent.DataOraModifica = DateTime.UtcNow;

            // TODO: Handle participants update
            // existingEvent.Participants = await _unitOfWork.Users.GetByIdsAsync(request.ParticipantIds);

            _unitOfWork.Events.Update(existingEvent);
            await _unitOfWork.SaveChangesAsync();

            var eventDto = _mapper.Map<EventDto>(existingEvent);
            return Result<EventDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error updating event: {ex.Message}");
        }
    }
}
