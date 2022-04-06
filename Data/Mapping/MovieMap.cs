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
    public class MovieMap : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("MOVIES");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("ID")
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.Title)
                   .HasMaxLength(80)
                   .HasColumnName("TITLE");

            builder.Property(e => e.Year)
                   .HasColumnName("YEAR");

            builder.Property(e => e.Winner)
                   .HasColumnName("WINNER");
        }
    }
}
