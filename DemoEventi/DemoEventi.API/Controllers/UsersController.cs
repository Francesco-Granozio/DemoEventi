using DemoEventi.Application.DTOs;
using DemoEventi.Application.Users.Commands.CreateUser;
using DemoEventi.Application.Users.Commands.DeleteUser;
using DemoEventi.Application.Users.Commands.UpdateUser;
using DemoEventi.Application.Users.Queries.GetAllUsers;
using DemoEventi.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return StatusCode(500, new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving users", error = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        try
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the user", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateUserCommand { CreateUserDto = createUserDto };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
            }

            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateUserCommand { Id = id, UpdateUserDto = updateUserDto };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the user", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the user", error = ex.Message });
        }
    }
}
