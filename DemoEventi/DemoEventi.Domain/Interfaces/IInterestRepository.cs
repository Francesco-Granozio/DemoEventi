using DemoEventi.Domain.Entities;

namespace DemoEventi.Domain.Interfaces;

public interface IInterestRepository : IRepository<Interest>
{
    Task<IEnumerable<Interest>> GetByIdsAsync(IEnumerable<Guid> ids);
}
