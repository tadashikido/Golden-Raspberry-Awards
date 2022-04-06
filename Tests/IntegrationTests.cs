using Domain.DTO;
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

            var url = "/Movie";

            var result = await client.GetAsync(url);
            var consecutiveProducersAwards = await client.GetFromJsonAsync<ConsecutiveProducersAwardsDto>(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(consecutiveProducersAwards);
            Assert.Empty(consecutiveProducersAwards.Max);
            Assert.Empty(consecutiveProducersAwards.Min);
        }
    }
}
