using Data.Context;
using Domain.DTO;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class IntegrationTests : IClassFixture<Startup<Application.Startup>>
    {
        private readonly Startup<Application.Startup> _factory;

        public IntegrationTests(Startup<Application.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestResponse()
        {
            var client = _factory.CreateClient();

            InsertDataForTests();

            var url = "/Movie";

            var result = await client.GetAsync(url);
            var consecutiveProducersAwards = await client.GetFromJsonAsync<ConsecutiveProducersAwardsDto>(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(consecutiveProducersAwards);
            Assert.Equal(2, consecutiveProducersAwards.Min.Count);
            Assert.Single(consecutiveProducersAwards.Max);
        }

        [Fact]
        public async Task TestResponse_WhenTableIsEmpty()
        {
            var client = _factory.CreateClient();

            RemoveDataForTests();

            var url = "/Movie";

            var result = await client.GetAsync(url);
            var consecutiveProducersAwards = await client.GetFromJsonAsync<ConsecutiveProducersAwardsDto>(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(consecutiveProducersAwards);
            Assert.Empty(consecutiveProducersAwards.Max);
            Assert.Empty(consecutiveProducersAwards.Min);
        }

        private void RemoveDataForTests()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<BaseContext>();

            db.RemoveRange(db.Movies);

            db.SaveChanges();
        }

        private void InsertDataForTests()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<BaseContext>();

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
                Year = 1988,
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
        }
    }
}
