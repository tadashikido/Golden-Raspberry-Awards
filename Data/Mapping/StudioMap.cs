using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class StudioMap : IEntityTypeConfiguration<Studio>
    {
        public void Configure(EntityTypeBuilder<Studio> builder)
        {
            builder.ToTable("STUDIOS");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("ID")
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.Name)
                   .HasMaxLength(80)
                   .HasColumnName("NAME");

            builder.HasMany<Movie>()
                   .WithOne(x => x.Studio)
                   .HasForeignKey(x => x.StudioId);
        }
    }
}
