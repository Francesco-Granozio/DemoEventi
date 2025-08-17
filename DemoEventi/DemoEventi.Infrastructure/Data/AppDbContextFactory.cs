using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DemoEventi.Infrastructure.Data;

/// <summary>
/// Design-time factory for AppDbContext to enable EF Core migrations.
/// Reads configuration from the API project's appsettings.json.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Get the current directory (Infrastructure)
        var infrastructurePath = Directory.GetCurrentDirectory();

        // Navigate to the API project directory
        var apiPath = Path.Combine(infrastructurePath, "..", "DemoEventi.API");

        // If we're not in the expected structure, try to find the API project
        if (!Directory.Exists(apiPath))
        {
            // Try to find the API project by looking for appsettings.json in parent directories
            var currentDir = infrastructurePath;
            while (currentDir != null && !Directory.Exists(Path.Combine(currentDir, "DemoEventi.API")))
            {
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            if (currentDir != null)
            {
                apiPath = Path.Combine(currentDir, "DemoEventi.API");
            }
        }

        // Build configuration from the API project's appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Get the connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found. " +
                $"Ensure appsettings.json exists in: {apiPath}");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
