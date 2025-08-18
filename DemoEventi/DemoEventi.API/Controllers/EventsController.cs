using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Get all events with optional search and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetEvents(
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _eventService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(500, new { message = result.Error });
            }

            var events = result.Value!.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                events = events.Where(e => 
                    e.Name!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    e.Location!.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply pagination
            var totalCount = events.Count();
            var pagedEvents = events
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                events = pagedEvents,
                pagination = new
                {
                    currentPage = page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    hasNextPage = page < Math.Ceiling((double)totalCount / pageSize),
                    hasPreviousPage = page > 1
                }
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving events", error = ex.Message });
        }
    }

    /// <summary>
    /// Get event by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventDto>> GetEvent(Guid id)
    {
        try
        {
            var result = await _eventService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the event", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto createEventDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.CreateAsync(createEventDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetEvent), new { id = result.Value.Id }, result.Value);
            }

            if (result.ValidationErrors.Any())
            {
                return BadRequest(new { message = "Validation failed", errors = result.ValidationErrors });
            }

            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the event", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing event
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] CreateEventDto updateEventDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.UpdateAsync(id, updateEventDto);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            if (result.ValidationErrors.Any())
            {
                return BadRequest(new { message = "Validation failed", errors = result.ValidationErrors });
            }

            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the event", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        try
        {
            var result = await _eventService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the event", error = ex.Message });
        }
    }

    /// <summary>
    /// Add participants to an event
    /// </summary>
    [HttpPost("{id:guid}/participants")]
    public async Task<IActionResult> AddParticipants(Guid id, [FromBody] AddParticipantsDto addParticipantsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.AddParticipantsAsync(id, addParticipantsDto.UserIds);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding participants", error = ex.Message });
        }
    }
}

/// <summary>
/// DTO for adding participants to an event
/// </summary>
public class AddParticipantsDto
{
    [Required(ErrorMessage = "User IDs are required")]
    public IEnumerable<Guid> UserIds { get; set; } = new List<Guid>();
}
