using awesome_pizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Configurations
{
    public class IngredientPizzaConfiguration : IEntityTypeConfiguration<IngredientPizza>
    {
        public void Configure(EntityTypeBuilder<IngredientPizza> builder)
        {
            builder.ToTable(nameof(IngredientPizza));

            builder.HasKey(x => new { x.PizzaId, x.IngredientId });

            builder.HasOne(x => x.Pizza)
                .WithMany(x => x.IngredientPizzas)
                .HasForeignKey(x => x.PizzaId);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.IngredientPizzas)
                .HasForeignKey(x => x.IngredientId);
        }
    }
}
