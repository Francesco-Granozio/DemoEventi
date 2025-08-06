using DemoEventi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Domain.Entities;

public class Event : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }

    // Users who have registered for the event
    public ICollection<User> Participants { get; set; } = new List<User>();
}