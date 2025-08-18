using DemoEventi.Domain.Common;

namespace DemoEventi.Domain.Entities;
public class User : AuditableEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfileImageUrl { get; set; }

    // Interests chosen by the user
    public ICollection<Interest> Interests { get; set; } = new List<Interest>();

    // Events the user will participate in
    public ICollection<Event> Events { get; set; } = new List<Event>();
}