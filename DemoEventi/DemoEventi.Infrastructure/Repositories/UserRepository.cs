using DemoEventi.Domain.Entities;
using DemoEventi.Domain.Interfaces;
using DemoEventi.Infrastructure.Data;

namespace DemoEventi.Infrastructure.Repositories;

public class UserRepository : EfRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    // Place custom user queries here
}