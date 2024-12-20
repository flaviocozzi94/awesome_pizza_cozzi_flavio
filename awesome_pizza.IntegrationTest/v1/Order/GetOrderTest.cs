using awesome_pizza.Domain.Entities.Enumerators;
using awesome_pizza.Domain.Entities;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using System.Net.Http.Json;
using awesome_pizza.Application.Order;
using Microsoft.EntityFrameworkCore;
using awesome_pizza_cozzi_flavio;

namespace awesome_pizza.IntegrationTest.v1.Order
{
    [Collection("pizza collection")]
    public class GetOrderTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public GetOrderTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();

            context = factory.GetDbContext();

        }

        [Fact]
        public async Task CanGetOneOrder_ReturnOrder()
        {
            // Arrange
            var order = await context.Orders.FirstAsync();

            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/orders/{order.Id.ToString()}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);

            var responseObject = await response.Content.ReadFromJsonAsync<GetOrderByIdResponse>();
            Assert.NotNull(responseObject);

            Assert.Equal(order.Id, responseObject.Id);
            Assert.Equal(order.InsertDate, responseObject.InsertDate);
            Assert.Equal(order.Status, responseObject.Status);

        }

        [Theory]
        [InlineData(OrderStatus.Open)]
        [InlineData(OrderStatus.Preparing)]
        [InlineData(OrderStatus.Delivering)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Deleted)]
        public async Task CanGetOrdersByStatus_ReturnListOfOrders(OrderStatus orderStatus)
        {
            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/orders?orderStatus={(int)orderStatus}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = await response.Content.ReadFromJsonAsync<GetOrdersResponseWrapper>();
            Assert.NotNull(response);
            Assert.All(responseObject.Data, x => Assert.Equal(orderStatus, x.OrderStatus));
        }

        [Fact]
        public async Task CanGetOrders_ReturnListOfOrders()
        {
            // Act
            var response = await client.GetAsync($"/api/v{apiVersion}/orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = await response.Content.ReadFromJsonAsync<GetOrdersResponseWrapper>();
            Assert.NotNull(response);
        }
    }
}
