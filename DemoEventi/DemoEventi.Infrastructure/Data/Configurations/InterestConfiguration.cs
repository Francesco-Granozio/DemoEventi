using DemoEventi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Infrastructure.Data.Configurations;

public class InterestConfiguration : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(i => i.Description)
               .HasMaxLength(500);

        builder.Property(i => i.DataOraCreazione)
               .IsRequired();
        builder.Property(i => i.DataOraModifica);
    }
}