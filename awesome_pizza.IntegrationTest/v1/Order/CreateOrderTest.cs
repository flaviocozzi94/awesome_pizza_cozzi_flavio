using awesome_pizza.Application.Order;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using awesome_pizza_cozzi_flavio;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace awesome_pizza.IntegrationTest.v1.Order
{
    [Collection("pizza collection")]
    public class CreateOrderTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public CreateOrderTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
            context = factory.GetDbContext();

        }

        [Fact]
        public async Task CanCreateOrderAsync_ReturnCreatedResponse()
        {
            //  Arrange
            var orderRequest = new CreateOrderRequest
            {
                Pizzas = new List<int> { 1, 2, 3 }
            };

            //  Act
            var response = await client.PostAsJsonAsync($"/api/v{apiVersion}/orders", orderRequest);

            //  Assert
            response.EnsureSuccessStatusCode();

            var responseObject = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
            Assert.NotNull(responseObject);
            Assert.True(responseObject.OrderId != Guid.Empty);

            var order = await context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == responseObject.OrderId);
            Assert.NotNull(order);
            Assert.False(order.IsDeleted);
        }

    }
}
