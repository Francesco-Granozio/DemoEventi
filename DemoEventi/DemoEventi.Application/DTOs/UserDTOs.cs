namespace DemoEventi.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<Guid>? InterestIds { get; set; }
    
    // Computed properties for UI
    public string FullName => $"{FirstName} {LastName}".Trim();
    public int InterestCount => InterestIds?.Count() ?? 0;
}

public class CreateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IEnumerable<Guid>? InterestIds { get; set; }
}
