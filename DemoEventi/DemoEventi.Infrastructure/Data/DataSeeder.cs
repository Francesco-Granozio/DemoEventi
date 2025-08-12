using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoEventi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed interests if they don't exist
        if (!await context.Set<Interest>().AnyAsync())
        {
            var interests = new List<Interest>
            {
                new() { Id = Guid.NewGuid(), Name = "Technology", Description = "Tech and programming" },
                new() { Id = Guid.NewGuid(), Name = "Sports", Description = "Physical activities" },
                new() { Id = Guid.NewGuid(), Name = "Music", Description = "Musical interests" },
                new() { Id = Guid.NewGuid(), Name = "Travel", Description = "Travel and exploration" },
                new() { Id = Guid.NewGuid(), Name = "Cooking", Description = "Culinary arts" },
                new() { Id = Guid.NewGuid(), Name = "Reading", Description = "Books and literature" },
                new() { Id = Guid.NewGuid(), Name = "Gaming", Description = "Video games and board games" },
                new() { Id = Guid.NewGuid(), Name = "Art", Description = "Visual arts and creativity" }
            };

            foreach (var interest in interests)
            {
                interest.DataOraCreazione = DateTime.UtcNow;
            }

            await context.Set<Interest>().AddRangeAsync(interests);
            await context.SaveChangesAsync();
        }

        // Seed users if they don't exist
        if (!await context.Set<User>().AnyAsync())
        {
            var interests = await context.Set<Interest>().ToListAsync();
            var random = new Random();

            var users = new List<User>
            {
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    FirstName = "John", 
                    LastName = "Doe",
                    Interests = interests.Take(3).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    FirstName = "Jane", 
                    LastName = "Smith",
                    Interests = interests.Skip(2).Take(2).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    FirstName = "Bob", 
                    LastName = "Johnson",
                    Interests = interests.Take(1).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    FirstName = "Alice", 
                    LastName = "Brown",
                    Interests = interests.Skip(4).Take(2).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    FirstName = "Charlie", 
                    LastName = "Wilson",
                    Interests = interests.Skip(1).Take(3).ToList()
                }
            };

            foreach (var user in users)
            {
                user.DataOraCreazione = DateTime.UtcNow;
            }

            await context.Set<User>().AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // Seed events if they don't exist
        if (!await context.Set<Event>().AnyAsync())
        {
            var users = await context.Set<User>().ToListAsync();
            var random = new Random();

            var events = new List<Event>
            {
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Tech Conference 2024", 
                    Location = "San Francisco, CA",
                    StartDate = DateTime.Now.AddDays(30),
                    Participants = users.Take(3).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Music Festival", 
                    Location = "Austin, TX",
                    StartDate = DateTime.Now.AddDays(45),
                    Participants = users.Skip(1).Take(2).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Cooking Workshop", 
                    Location = "New York, NY",
                    StartDate = DateTime.Now.AddDays(15),
                    Participants = users.Skip(2).Take(2).ToList()
                },
                new() 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Sports Tournament", 
                    Location = "Los Angeles, CA",
                    StartDate = DateTime.Now.AddDays(60),
                    Participants = users.Take(2).ToList()
                }
            };

            foreach (var evt in events)
            {
                evt.DataOraCreazione = DateTime.UtcNow;
            }

            await context.Set<Event>().AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
}
