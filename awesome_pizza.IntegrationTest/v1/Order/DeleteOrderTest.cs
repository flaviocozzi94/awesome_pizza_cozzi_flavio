using awesome_pizza.Application.Order;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using awesome_pizza_cozzi_flavio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.IntegrationTest.v1.Order
{
    [Collection("pizza collection")]
    public class DeleteOrderTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public DeleteOrderTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
            context = factory.GetDbContext();
        }

        [Fact]
        public async Task CanDeleteOrderAsync_ReturnOrderId()
        {
            //  Arrange
            var idOrder = Guid.NewGuid();
            context.Orders.Add(new Domain.Entities.Order
            {
                Id = idOrder,
                InsertDate = DateTime.UtcNow,
                OrderPizzas = new List<Domain.Entities.OrderPizza>
                {
                    new Domain.Entities.OrderPizza
                    {
                        OrderId = idOrder,
                        PizzaId = 1
                    },
                    new Domain.Entities.OrderPizza
                    {
                        OrderId = idOrder,
                        PizzaId = 3
                    }
                },
                Status = Domain.Entities.Enumerators.OrderStatus.Open
            });
            await context.SaveChangesAsync();

            //  Act
            var response = await client.DeleteAsync($"/api/v{apiVersion}/orders/{idOrder.ToString()}");

            //  Assert
            response.EnsureSuccessStatusCode();

            var deletedOrder = await context.Orders.AsNoTracking().FirstAsync(x => x.Id == idOrder);
            Assert.True(deletedOrder.IsDeleted);
        }
    }
}
