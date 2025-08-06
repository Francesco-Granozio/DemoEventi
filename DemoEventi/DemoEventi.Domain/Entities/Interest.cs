using DemoEventi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Domain.Entities;

/// <summary>
/// Catalog of interests. Predefined set, not editable by users.
/// </summary>
public class Interest : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigation: users who have this interest
    public ICollection<User> Users { get; set; } = new List<User>();
}

