using CrossCutting.DependencyInjection;
using Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using System.IO;
using Domain.Entities;

namespace Application
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services, Configuration);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            InitializeDatabase(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void InitializeDatabase(IApplicationBuilder app)
        {
            File.Delete("Database.db");

            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseContext>();
            db.Database.Migrate();

            if (!File.Exists("movielist.csv")) return;

            using var stream = new StreamReader("movielist.csv");

            var records = CsvHelpers.MountRecordsFromCsv<MovieDto>(stream);

            var producers = new List<Producer>();
            var studios = new List<Studio>();
            var movies = new List<Movie>();

            records.ForEach(x =>
            {
                var producer = producers.Where(y => y.Name == x.Producer).FirstOrDefault();

                if (producer == null)
                {
                    producer = new Producer
                    {
                        Name = x.Producer
                    };

                    producers.Add(producer);
                }

                var studio = studios.Where(y => y.Name == x.Studio).FirstOrDefault();

                if (studio == null)
                {
                    studio = new Studio
                    {
                        Name = x.Studio
                    };

                    studios.Add(studio);
                }

                var movie = new Movie
                {
                    Year = x.Year,
                    Title = x.Title,
                    Winner = x.Winner == "yes",
                    Producer = producer,
                    Studio = studio
                };

                movies.Add(movie);
            });

            db.Movies.AddRange(movies);
            db.SaveChanges();
        }
    }
}
