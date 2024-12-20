namespace awesome_pizza.Domain.Entities
{
    public class Ingredient
    {
        public Ingredient()
        {
            IngredientPizzas = new HashSet<IngredientPizza>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime InsertDate { get; set; }
        public ICollection<IngredientPizza> IngredientPizzas { get; set; }
    }
}
