using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;
using DemoEventi.Infrastructure.Repositories;

namespace DemoEventi.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IEventRepository Events { get; }
    public IInterestRepository Interests { get; }

    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Events = new EventRepository(context);
        Interests = new InterestRepository(context);
    }

    public async Task<int> CommitAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}