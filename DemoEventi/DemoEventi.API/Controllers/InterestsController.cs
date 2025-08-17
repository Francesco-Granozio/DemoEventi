using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interests.Queries.GetAllInterests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InterestDto>>> GetInterests()
    {
        var query = new GetAllInterestsQuery();
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
