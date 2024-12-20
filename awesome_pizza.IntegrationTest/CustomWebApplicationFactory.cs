using awesome_pizza.Domain.Entities.Enumerators;
using awesome_pizza.Domain.Entities;
using awesome_pizza.Infrastructure.Persistence.EfCore;
using awesome_pizza_cozzi_flavio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace awesome_pizza.IntegrationTest
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : Program
    {
        public Guid OrderId { get; private set; }
        private static bool isAlreadyCreatedDb = false;
        private static string dbName = "MyDbTest";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IDbContextOptionsConfiguration<AwesomePizzaDbContext>));
                //typeof(DbContextOptions<AwesomePizzaDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<AwesomePizzaDbContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    if (!isAlreadyCreatedDb)
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AwesomePizzaDbContext>();
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        InitializeDb(context).GetAwaiter().GetResult();
                        //isAlreadyCreatedDb = true;
                    }
                }
            });
        }

        public AwesomePizzaDbContext? GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AwesomePizzaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AwesomePizzaDbContext(options);
        }

        private async Task InitializeDb(AwesomePizzaDbContext context)
        {
            context.Pizzas.AddRange(new List<Domain.Entities.Pizza>
                {
                    new Domain.Entities.Pizza { Id = 1, Name = "Margherita", Price = 5 , IsAvailable = true, InsertDate = DateTime.UtcNow,
                        IngredientPizzas= new List<IngredientPizza>{
                        new IngredientPizza{IngredientId=1, PizzaId=1},
                        new IngredientPizza{IngredientId=2, PizzaId=1},
                    }
                    },
                    new Domain.Entities.Pizza { Id = 2, Name = "Marinara", Price = 5 , IsAvailable = false, InsertDate = DateTime.UtcNow,
                        IngredientPizzas= new List<IngredientPizza>{
                        new IngredientPizza{IngredientId=1, PizzaId=2}
                    }
                    },
                    new Domain.Entities.Pizza { Id = 3, Name = "Peperoni", Price = 6.50 ,IsAvailable = true, InsertDate = DateTime.UtcNow,
                       IngredientPizzas= new List<IngredientPizza>{
                        new IngredientPizza{IngredientId=1, PizzaId=3},
                        new IngredientPizza{IngredientId=2, PizzaId=3},
                        new IngredientPizza{IngredientId=5, PizzaId=3},
                    }}
                });

            OrderId = Guid.NewGuid();
            context.Orders.Add(new Domain.Entities.Order
            {
                Id = OrderId,
                Status = OrderStatus.Open,
                InsertDate = DateTime.UtcNow,
                OrderPizzas = new List<OrderPizza>
                    {
                        new OrderPizza {OrderId = OrderId, PizzaId = 1 },
                        new OrderPizza {OrderId = OrderId, PizzaId = 2 }
                    }
            });

            await context.SaveChangesAsync();
        }
    }
}
