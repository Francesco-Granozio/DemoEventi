using DemoEventi.Application.DTOs;
using DemoEventi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var result = await _userService.GetAllAsync();
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
            var result = await _userService.GetByIdAsync(id);
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

            var result = await _userService.CreateAsync(createUserDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
            }

            if (result.ValidationErrors.Any())
            {
                return BadRequest(new { message = "Validation failed", errors = result.ValidationErrors });
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
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CreateUserDto updateUserDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateAsync(id, updateUserDto);
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
            var result = await _userService.DeleteAsync(id);
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
