using AutoMapper;
using DemoEventi.Application.Common;
using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using DemoEventi.Application.Validators;
using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using FluentValidation;

namespace DemoEventi.Application.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEventDto> _validator;
    private readonly IUserRepository _userRepository;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateEventDto> validator, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _userRepository = userRepository;
    }

    public async Task<Result<EventDto>> CreateAsync(CreateEventDto dto)
    {
        try
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<EventDto>.ValidationFailure(errors);
            }

            var ev = _mapper.Map<Event>(dto);
            ev.DataOraCreazione = DateTime.UtcNow;

            // Handle participants if provided
            if (dto.ParticipantIds != null && dto.ParticipantIds.Any())
            {
                var participants = await _userRepository.GetByIdsAsync(dto.ParticipantIds);
                ev.Participants = participants.ToList();
            }

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

    public async Task<Result<IEnumerable<EventDto>>> GetAllAsync()
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

    public async Task<Result<EventDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(id);
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

    public async Task<Result<EventDto>> UpdateAsync(Guid id, CreateEventDto dto)
    {
        try
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<EventDto>.ValidationFailure(errors);
            }

            var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
            if (existingEvent == null)
            {
                return Result<EventDto>.Failure("Event not found");
            }

            // Update properties
            existingEvent.Name = dto.Name;
            existingEvent.Location = dto.Location;
            existingEvent.StartDate = dto.StartDate;
            existingEvent.DataOraModifica = DateTime.UtcNow;

            // Handle participants update
            if (dto.ParticipantIds != null)
            {
                var participants = await _userRepository.GetByIdsAsync(dto.ParticipantIds);
                existingEvent.Participants = participants.ToList();
            }
            else
            {
                existingEvent.Participants.Clear();
            }

            _unitOfWork.Events.Update(existingEvent);
            await _unitOfWork.CommitAsync();

            var eventDto = _mapper.Map<EventDto>(existingEvent);
            return Result<EventDto>.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Failure($"Error updating event: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        try
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(id);
            if (ev == null)
            {
                return Result.Failure("Event not found");
            }

            _unitOfWork.Events.Delete(ev);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting event: {ex.Message}");
        }
    }

    public async Task<Result> AddParticipantsAsync(Guid eventId, IEnumerable<Guid> userIds)
    {
        try
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (ev == null)
            {
                return Result.Failure("Event not found");
            }

            var users = await _userRepository.GetByIdsAsync(userIds);
            foreach (var user in users)
            {
                if (!ev.Participants.Any(p => p.Id == user.Id))
                {
                    ev.Participants.Add(user);
                }
            }

            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error adding participants: {ex.Message}");
        }
    }
}
