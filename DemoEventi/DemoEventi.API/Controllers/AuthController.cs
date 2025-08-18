using DemoEventi.Application.DTOs;
using DemoEventi.Application.Users.Commands.CreateUser;
using DemoEventi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DemoEventi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public AuthController(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "Email and password are required"
                });
            }

            // Find user by email
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "Invalid email or password"
                });
            }

            // Verify password
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "Invalid email or password"
                });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                InterestIds = user.Interests?.Select(i => i.Id).ToList() ?? new List<Guid>()
            };

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                User = userDto,
                Token = GenerateJwtToken(user.Id.ToString()) // Simple token for demo
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new AuthResponseDto
            {
                IsSuccess = false,
                Error = $"Login failed: {ex.Message}"
            });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (string.IsNullOrEmpty(registerDto.Email) || 
                string.IsNullOrEmpty(registerDto.Password) ||
                string.IsNullOrEmpty(registerDto.FirstName) ||
                string.IsNullOrEmpty(registerDto.LastName))
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "All fields are required"
                });
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "Passwords do not match"
                });
            }

            // Check if user already exists
            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(u => u.Email.Equals(registerDto.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = "User with this email already exists"
                });
            }

            // Create user
            var createCommand = new CreateUserCommand
            {
                CreateUserDto = new CreateUserDto
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Password = registerDto.Password, // Don't hash here - let the handler do it
                    InterestIds = new List<Guid>()
                }
            };

            var result = await _mediator.Send(createCommand);
            if (!result.IsSuccess)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Error = result.Error
                });
            }

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                User = result.Value,
                Token = GenerateJwtToken(result.Value!.Id.ToString())
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new AuthResponseDto
            {
                IsSuccess = false,
                Error = $"Registration failed: {ex.Message}"
            });
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "DemoEventi_Salt"));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashToVerify = HashPassword(password);
        return hashToVerify == hashedPassword;
    }

    private string GenerateJwtToken(string userId)
    {
        // For demo purposes, return a simple token
        // In production, use proper JWT with signing
        return $"Bearer_{userId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }
}
