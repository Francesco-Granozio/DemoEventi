using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;
using DemoEventi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

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

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public void Dispose()
        => _context.Dispose();
}