using Data.Mapping;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(new MovieMap().Configure);
            modelBuilder.Entity<Producer>(new ProducerMap().Configure);
            modelBuilder.Entity<Studio>(new StudioMap().Configure);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Studio> Studios { get; set; }
    }
}
