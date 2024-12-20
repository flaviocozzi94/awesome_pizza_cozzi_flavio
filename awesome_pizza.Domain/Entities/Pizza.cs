using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Entities
{
    public class Pizza
    {
        public Pizza()
        {
            IngredientPizzas = new HashSet<IngredientPizza>();
            OrderPizzas = new HashSet<OrderPizza>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime InsertDate { get; set; }

        public ICollection<IngredientPizza> IngredientPizzas { get; set; }
        public ICollection<OrderPizza> OrderPizzas { get; set; }
    }
}
