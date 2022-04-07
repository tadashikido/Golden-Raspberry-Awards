using Data.Context;
using Domain.DTO;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

            var movie = new Movie
            {
                Year = 1980,
                Title = "Can't Stop the Music",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie, Producer = producer });
            db.Add(new MovieStudio { Movie = movie, Studio = studio });

            var movie2 = new Movie
            {
                Year = 1981,
                Title = "Cruising",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie2, Producer = producer });
            db.Add(new MovieStudio { Movie = movie2, Studio = studio });

            var movie3 = new Movie
            {
                Year = 1985,
                Title = "Butterfly",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie3, Producer = producer2 });
            db.Add(new MovieStudio { Movie = movie3, Studio = studio });

            var movie4 = new Movie
            {
                Year = 1986,
                Title = "Megaforce",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie4, Producer = producer2 });
            db.Add(new MovieStudio { Movie = movie4, Studio = studio });

            var movie5 = new Movie
            {
                Year = 1988,
                Title = "Two of a Kind",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie5, Producer = producer2 });
            db.Add(new MovieStudio { Movie = movie5, Studio = studio });

            var movie6 = new Movie
            {
                Year = 1997,
                Title = "Rhinestone",
                Winner = true
            };

            db.Add(new MovieProducer { Movie = movie6, Producer = producer2 });
            db.Add(new MovieStudio { Movie = movie6, Studio = studio });

            db.SaveChanges();
        }
    }
}
