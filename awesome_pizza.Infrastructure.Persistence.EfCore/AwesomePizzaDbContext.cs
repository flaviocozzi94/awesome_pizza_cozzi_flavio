using awesome_pizza.Domain.Entities;
using awesome_pizza.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace awesome_pizza.Infrastructure.Persistence.EfCore
{
    public class AwesomePizzaDbContext : DbContext, IUnitOfWork
    {
        public AwesomePizzaDbContext(DbContextOptions<AwesomePizzaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(builder);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<OrderPizza> OrderPizzas { get; set; }
    }
}
