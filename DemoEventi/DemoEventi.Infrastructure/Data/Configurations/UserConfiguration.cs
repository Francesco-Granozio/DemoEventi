using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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