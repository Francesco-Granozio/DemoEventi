namespace DemoEventi.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid>? ParticipantIds { get; set; }
    public IEnumerable<UserDto>? Participants { get; set; }

    // Computed properties for UI
    public int ParticipantCount => ParticipantIds?.Count() ?? 0;
    public string ParticipantCountText => ParticipantCount == 1 ? "1 participant" : $"{ParticipantCount} participants";
}

public class CreateEventDto
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid>? ParticipantIds { get; set; }
}

public class EventSearchDto
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
