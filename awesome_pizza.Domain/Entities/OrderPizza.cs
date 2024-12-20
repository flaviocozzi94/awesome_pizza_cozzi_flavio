using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Entities
{
    public class OrderPizza
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int PizzaId { get; set; }
        public Order Order { get; set; }
        public Pizza Pizza { get; set; }
    }
}
