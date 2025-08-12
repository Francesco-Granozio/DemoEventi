using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace DemoEventi.Infrastructure.Data;

/// <summary>
/// Permette agli strumenti EF di istanziare AppDbContext design-time.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Individua la cartella corrente (Infrastructure)
        var basePath = Directory.GetCurrentDirectory();

        // Costruisci la configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Leggi la connection string
        var connStr = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connStr);

        return new AppDbContext(optionsBuilder.Options);
    }
}
