using awesome_pizza.Domain.Entities.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Entities
{
    public class Order
    {
        public Order()
        {
            OrderPizzas = new HashSet<OrderPizza>();
        }
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime InsertDate { get; set; }
        public ICollection<OrderPizza> OrderPizzas { get; set; }
        public bool IsDeleted { get; set; }
    }
}
