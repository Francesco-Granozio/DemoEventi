using DemoEventi.Domain.Entities;

namespace DemoEventi.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids);
}
