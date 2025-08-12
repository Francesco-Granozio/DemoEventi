using DemoEventi.Domain.Entities;

namespace DemoEventi.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    // Additional user-specific methods can be declared here
}
