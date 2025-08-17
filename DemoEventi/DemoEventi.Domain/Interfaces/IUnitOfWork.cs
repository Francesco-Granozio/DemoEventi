using Microsoft.EntityFrameworkCore.Storage;

namespace DemoEventi.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    IInterestRepository Interests { get; }
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}