using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(200);
        builder.Property(e => e.Location)
               .IsRequired()
               .HasMaxLength(200);
        builder.Property(e => e.StartDate)
               .IsRequired();

        builder.Property(e => e.DataOraCreazione)
               .IsRequired();
        builder.Property(e => e.DataOraModifica);
    }
}