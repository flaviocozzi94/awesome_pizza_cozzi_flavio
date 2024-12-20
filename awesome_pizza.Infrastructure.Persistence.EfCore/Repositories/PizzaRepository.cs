using awesome_pizza.Domain.Entities;
using awesome_pizza.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Repositories
{
    public class PizzaRepository : GenericRepository<Domain.Entities.Pizza>, IPizzaRepository
    {
        private AwesomePizzaDbContext context;

        public PizzaRepository(AwesomePizzaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Pizza>> GetAllPizzaAsync(bool includeUnavailablePizza = false)
        {
            if (includeUnavailablePizza)
                return await context.Pizzas
                    .Include(x => x.IngredientPizzas).ThenInclude(x => x.Ingredient)
                    .AsNoTracking()
                    .ToListAsync();
            else
                return await context.Pizzas
                .Include(x => x.IngredientPizzas).ThenInclude(x => x.Ingredient)
                .AsNoTracking()
                .Where(x => x.IsAvailable == true)
                .ToListAsync();
        }

        public async Task<Pizza> GetPizzaByIdAsync(int id)
        {
            return await context.Pizzas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
