using System.Linq.Expressions;

namespace DemoEventi.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");

    Task<T> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}