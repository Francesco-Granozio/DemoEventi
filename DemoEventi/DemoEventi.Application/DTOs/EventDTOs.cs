namespace DemoEventi.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid>? ParticipantIds { get; set; }
    
    // Computed properties for UI
    public int ParticipantCount => ParticipantIds?.Count() ?? 0;
}

public class CreateEventDto
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid>? ParticipantIds { get; set; }
}
