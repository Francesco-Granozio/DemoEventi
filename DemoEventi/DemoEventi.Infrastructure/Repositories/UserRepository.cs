using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoEventi.Infrastructure.Repositories;

public class UserRepository : EfRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Set<User>()
            .Where(u => ids.Contains(u.Id))
            .ToListAsync();
    }
}