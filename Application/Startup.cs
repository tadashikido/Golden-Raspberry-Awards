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

            var producersList = new List<Producer>();
            var studiosList = new List<Studio>();

            records.ForEach(x =>
            {
                var movie = new Movie
                {
                    Year = x.Year,
                    Title = x.Title,
                    Winner = x.Winner == "yes"
                };

                var producers = x.Producer.Split(new string[] { ",", " and " }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

                foreach (var p in producers)
                {
                    var producer = producersList.Where(y => y.Name == p).FirstOrDefault();

                    if (producer == null)
                    {
                        producer = new Producer
                        {
                            Name = x.Producer
                        };

                        producersList.Add(producer);
                    }

                    db.Add(new MovieProducer
                    {
                        Movie = movie,
                        Producer = producer
                    });
                }

                var studios = x.Studio.Split(new string[] { ",", " and " }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

                foreach (var s in studios)
                {
                    var studio = studiosList.Where(y => y.Name == s).FirstOrDefault();

                    if (studio == null)
                    {
                        studio = new Studio
                        {
                            Name = x.Producer
                        };

                        studiosList.Add(studio);
                    }

                    db.Add(new MovieStudio
                    {
                        Movie = movie,
                        Studio = studio
                    });
                }
            });

            db.SaveChanges();
        }
    }
}
