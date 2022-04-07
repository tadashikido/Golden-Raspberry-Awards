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
    public class MovieProducerMap : IEntityTypeConfiguration<MovieProducer>
    {
        public void Configure(EntityTypeBuilder<MovieProducer> builder)
        {
            builder.ToTable("MOVIES_PRODUCERS");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("ID")
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.MovieId)
                   .HasColumnName("ID_MOVIE");

            builder.Property(e => e.ProducerId)
                   .HasColumnName("ID_PRODUCER");
        }
    }
}
