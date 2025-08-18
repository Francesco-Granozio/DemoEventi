using DemoEventi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/participants")]
public class EventParticipationController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EventParticipationController(
        IEventRepository eventRepository, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> JoinEvent(Guid eventId, Guid userId)
    {
        try
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Check if user is already participating
            if (eventEntity.Participants.Any(p => p.Id == userId))
            {
                return BadRequest(new { message = "User is already participating in this event" });
            }

            // Add user to event participants
            eventEntity.Participants.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Successfully joined the event" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while joining the event", error = ex.Message });
        }
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> LeaveEvent(Guid eventId, Guid userId)
    {
        try
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Check if user is participating
            var participant = eventEntity.Participants.FirstOrDefault(p => p.Id == userId);
            if (participant == null)
            {
                return BadRequest(new { message = "User is not participating in this event" });
            }

            // Remove user from event participants
            eventEntity.Participants.Remove(participant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Successfully left the event" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while leaving the event", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetParticipants(Guid eventId)
    {
        try
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            var participants = eventEntity.Participants.Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.Email,
                p.ProfileImageUrl,
                FullName = $"{p.FirstName} {p.LastName}".Trim()
            }).ToList();

            return Ok(new
            {
                eventId,
                participantCount = participants.Count,
                participants
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving participants", error = ex.Message });
        }
    }
}
