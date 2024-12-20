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
    public class CreatePizzaTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public CreatePizzaTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
            context = factory.GetDbContext();

        }

        [Fact]
        public async Task CanCreatePizzaAsync_ReturnCreatedResponse()
        {
            //  Arrange
            var pizzaRequest = new CreatePizzaRequest
            {
                Name = "4 stagioni",
                Description = "Pizza 4 stagioni",
                Price = 7.5
            };

            //  Act
            var response = await client.PostAsJsonAsync($"/api/v{apiVersion}/pizzas", pizzaRequest);

            //  Assert
            response.EnsureSuccessStatusCode();

            var responseObject = await response.Content.ReadFromJsonAsync<CreatePizzaResponse>();

            Assert.NotNull(responseObject);
            Assert.True(responseObject.PizzaId != 0);

            var pizza = await context.Pizzas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == responseObject.PizzaId);
            Assert.NotNull(pizza);
        }
    }
}
