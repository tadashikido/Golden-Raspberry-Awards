using CrossCutting.DependencyInjection;
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

                ConfigureService.ConfigureDependenciesService(services);
                ConfigureRepository.ConfigureDependenciesRepository(services, configuration);

                services.AddControllers();

                services.AddHttpContextAccessor();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var db = scopedServices.GetRequiredService<BaseContext>();

                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
