using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoEventi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.InterestConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.EventConfiguration());
    }
}