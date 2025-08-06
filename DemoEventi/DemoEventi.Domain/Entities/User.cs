using DemoEventi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Domain.Entities;
public class User : AuditableEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Interests chosen by the user
    public ICollection<Interest> Interests { get; set; } = new List<Interest>();

    // Events the user will participate in
    public ICollection<Event> Events { get; set; } = new List<Event>();
}