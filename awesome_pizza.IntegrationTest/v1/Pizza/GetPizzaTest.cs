using awesome_pizza.Application.Order;
using awesome_pizza.Application.Pizza;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using awesome_pizza_cozzi_flavio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.IntegrationTest.v1.Pizza
{
    [Collection("pizza collection")]
    public class GetPizzaTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public GetPizzaTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
            context = factory.GetDbContext();
        }


        [Fact]
        public async Task CanGetOnePizzaByIdAsync_ReturnCreatedResponse()
        {
            // Arrange
            var pizza = await context.Pizzas.FirstOrDefaultAsync();

            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/pizzas/{pizza.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);

            var responseObject = await response.Content.ReadFromJsonAsync<GetPizzaByIdResponse>();
            Assert.NotNull(responseObject);

            Assert.Equal(pizza.Id, responseObject.Id);
            Assert.Equal(pizza.IsAvailable, responseObject.IsAvailable);
            Assert.Equal(pizza.Name, responseObject.Name);
            Assert.Equal(pizza.Price, responseObject.Price);
            Assert.Equal(pizza.Description, responseObject.Description);
        }

        [Fact]
        public async Task CanGetAllPizzasAsync_ReturnListOfPizzas()
        {
            // Arrange
            var pizza = await context.Pizzas.CountAsync();

            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/pizzas?includeUnavailablePizza=true");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);

            var responseObject = await response.Content.ReadFromJsonAsync<GetAllPizzaResponseWrapper>();
            Assert.NotNull(responseObject);
            Assert.NotEmpty(responseObject.Data);
            Assert.True(responseObject.Data.Count == 3);
        }

        [Fact]
        public async Task CanGetOnlyAvailablePizzasAsync_ReturnListOfPizzas()
        {
            // Arrange
            var pizza = await context.Pizzas.CountAsync();

            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/pizzas");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);

            var responseObject = await response.Content.ReadFromJsonAsync<GetAllPizzaResponseWrapper>();
            Assert.NotNull(responseObject);
            Assert.NotEmpty(responseObject.Data);
            Assert.True(responseObject.Data.Count == 2);
        }
    }
}
