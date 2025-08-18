using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEventi.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(255);
        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);
        builder.Property(u => u.ProfileImageUrl)
               .HasMaxLength(500);

        // Create unique index for email
        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.HasMany(u => u.Interests)
               .WithMany(i => i.Users)
               .UsingEntity(j => j.ToTable("UserInterests"));

        builder.HasMany(u => u.Events)
               .WithMany(e => e.Participants)
               .UsingEntity(j => j.ToTable("UserEvents"));

        builder.Property(u => u.DataOraCreazione)
               .IsRequired();
        builder.Property(u => u.DataOraModifica);
    }
}