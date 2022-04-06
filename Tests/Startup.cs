using Data.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class Startup<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BaseContext>));

                services.Remove(descriptor);

                var data = new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", "Data Source=Database.db;" }
                };

                var configuration = new ConfigurationBuilder().AddInMemoryCollection(data).Build();
                var conn = configuration.GetConnectionString("Default");

                services.AddDbContext<BaseContext>(options => options.UseSqlite(conn));

                services.AddControllers();

                services.AddHttpContextAccessor();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var db = scopedServices.GetRequiredService<BaseContext>();

                    InsertDataForTests(db);
                    db.Database.EnsureCreated();
                }
            });
        }

        private void InsertDataForTests(BaseContext db)
        {
            var producer = new Producer
            {
                Name = "Allan Carr"
            };

            var studio = new Studio
            {
                Name = "Associated Film Distribution"
            };

            var producer2 = new Producer
            {
                Name = "Jerry Weintraub"
            };

            db.Add(new Movie
            {
                Year = 1980,
                Title = "Can't Stop the Music",
                Producer = producer,
                Studio = studio,
                Winner = true
            });

            db.Add(new Movie
            {
                Year = 1981,
                Title = "Cruising",
                Producer = producer,
                Studio = studio,
                Winner = true
            });

            db.Add(new Movie
            {
                Year = 1985,
                Title = "Butterfly",
                Producer = producer2,
                Studio = studio,
                Winner = true
            });

            db.Add(new Movie
            {
                Year = 1986,
                Title = "Megaforce",
                Producer = producer2,
                Studio = studio,
                Winner = true
            });

            db.Add(new Movie
            {
                Year = 1987,
                Title = "Two of a Kind",
                Producer = producer2,
                Studio = studio,
                Winner = true
            });

            db.Add(new Movie
            {
                Year = 1997,
                Title = "Rhinestone",
                Producer = producer2,
                Studio = studio,
                Winner = true
            });

            db.SaveChanges();

            var producers2 = db.Movies.Where(x => x.Winner).ToList();
            var teste = 2;
        }
    }
}
