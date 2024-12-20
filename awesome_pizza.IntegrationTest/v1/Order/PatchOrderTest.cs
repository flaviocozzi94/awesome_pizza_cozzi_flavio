using awesome_pizza.Application.Order;
using awesome_pizza.Domain.Entities;
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
    public class PatchOrderTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private AwesomePizzaDbContext context;
        string apiVersion = "1";

        public PatchOrderTest(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
            context = factory.GetDbContext();
        }


        [Fact]
        public async Task CanPatchOrder_ReturnIdOfTheOrderEdited()
        {
            //  Arrange
            var idOrder = Guid.NewGuid();
            context.Orders.Add(new Domain.Entities.Order
            {
                Id = idOrder,
                InsertDate = DateTime.UtcNow,
                OrderPizzas = new List<OrderPizza>
                {
                    new OrderPizza
                    {
                        OrderId = idOrder,
                        PizzaId = 1
                    },
                    new OrderPizza
                    {
                        OrderId = idOrder,
                        PizzaId = 3
                    }
                },
                Status = Domain.Entities.Enumerators.OrderStatus.Open
            });
            await context.SaveChangesAsync();

            var orderRequest = new PatchStatusOrderRequest
            {
                Id = idOrder,
                OrderStatus = Domain.Entities.Enumerators.OrderStatus.Delivering
            };

            //  Act
            var response = await client.PatchAsJsonAsync($"/api/v{apiVersion}/orders/{idOrder.ToString()}", orderRequest);

            //  Assert
            response.EnsureSuccessStatusCode();
            var responseObject = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
            Assert.NotNull(responseObject);

            var editedOrder = await context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idOrder);
            Assert.Equal(orderRequest.Id, editedOrder.Id);
            Assert.Equal(orderRequest.OrderStatus, editedOrder.Status);
        }
    }
}
