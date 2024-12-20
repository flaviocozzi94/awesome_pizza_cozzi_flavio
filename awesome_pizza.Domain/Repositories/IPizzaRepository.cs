using awesome_pizza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Repositories
{
    public interface IPizzaRepository : IGenericRepository<Domain.Entities.Pizza>
    {
        Task<Pizza> GetPizzaByIdAsync(int id);
        Task<IEnumerable<Pizza>> GetAllPizzaAsync(bool includeUnavailablePizza = false);
    }
}
