using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid> ParticipantIds { get; set; }
}

public class CreateEventDto
{
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public IEnumerable<Guid> ParticipantIds { get; set; }
}
