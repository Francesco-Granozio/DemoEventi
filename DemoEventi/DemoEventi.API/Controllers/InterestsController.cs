using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterestsController : ControllerBase
{
    private readonly IInterestService _interestService;

    public InterestsController(IInterestService interestService)
    {
        _interestService = interestService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InterestDto>>> GetInterests()
    {
        var result = await _interestService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InterestDto>> GetInterest(Guid id)
    {
        var result = await _interestService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
}
