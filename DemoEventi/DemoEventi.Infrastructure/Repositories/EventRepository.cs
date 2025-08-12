using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;

namespace DemoEventi.Infrastructure.Repositories;

public class EventRepository : EfRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    // Place custom event queries here
}