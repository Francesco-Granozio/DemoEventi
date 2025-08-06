using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Infrastructure.Repositories;

public class EventRepository : EfRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    // Place custom event queries here
}