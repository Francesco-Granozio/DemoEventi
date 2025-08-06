using DemoEventi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    // Additional event-specific methods can be declared here
}