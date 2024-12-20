using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Entities
{
    public class IngredientPizza
    {
        public int PizzaId { get; set; }
        public int IngredientId { get; set; }
        public Pizza Pizza { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
