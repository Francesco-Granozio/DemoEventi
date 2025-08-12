using DemoEventi.Domain.Entities;

namespace DemoEventi.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    // Additional event-specific methods can be declared here
}