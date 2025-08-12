namespace DemoEventi.Application.DTOs;

public class InterestDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateInterestDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
