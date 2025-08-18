namespace DemoEventi.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public IEnumerable<Guid>? InterestIds { get; set; }

    // Computed properties for UI
    public string FullName => $"{FirstName} {LastName}".Trim();
    public int InterestCount => InterestIds?.Count() ?? 0;
}

public class CreateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfileImageUrl { get; set; }
    public IEnumerable<Guid>? InterestIds { get; set; }
}

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public IEnumerable<Guid>? InterestIds { get; set; }
}

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class RegisterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public UserDto? User { get; set; }
    public string? Error { get; set; }
}