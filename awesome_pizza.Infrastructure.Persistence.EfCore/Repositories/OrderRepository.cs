using awesome_pizza.Domain.Entities;
using awesome_pizza.Domain.Entities.Enumerators;
using awesome_pizza.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Repositories
{
    public class OrderRepository : GenericRepository<Domain.Entities.Order>, IOrderRepository
    {
        private AwesomePizzaDbContext context;

        public OrderRepository(AwesomePizzaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var order = await context.Orders.FindAsync(id);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(OrderStatus? orderStatus)
        {
            if (orderStatus == null)
                return await context.Orders
                       .Include(x => x.OrderPizzas).ThenInclude(x => x.Pizza)
                       .AsNoTracking()
                       .ToListAsync();
            else
                return await context.Orders
                    .Include(x => x.OrderPizzas).ThenInclude(x => x.Pizza)
                    .AsNoTracking()
                    .Where(x => x.Status == orderStatus)
                    .ToListAsync();
        }
    }
}
