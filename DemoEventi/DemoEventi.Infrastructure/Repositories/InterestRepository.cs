using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoEventi.Infrastructure.Repositories;

public class InterestRepository : EfRepository<Interest>, IInterestRepository
{
    public InterestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Interest>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Set<Interest>()
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();
    }
}
