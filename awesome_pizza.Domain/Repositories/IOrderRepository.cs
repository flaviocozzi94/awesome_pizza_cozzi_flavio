using awesome_pizza.Domain.Entities;
using awesome_pizza.Domain.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Repositories
{
    public interface IOrderRepository : IGenericRepository<Domain.Entities.Order>
    {
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersAsync(OrderStatus? orderStatus);
    }
}
