using awesome_pizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable(nameof(Ingredient));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.InsertDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.HasData(
            new Ingredient { Id = 1, Name = "Pomodo", Description = "Pomodoro", IsAvailable = true },
            new Ingredient { Id = 2, Name = "Mozzarella", Description = "Mozzarella ", IsAvailable = true },
            new Ingredient { Id = 3, Name = "Prosciutto", Description = "Prosciutto", IsAvailable = true },
            new Ingredient { Id = 4, Name = "Funghi", Description = "Funghi", IsAvailable = true },
            new Ingredient { Id = 5, Name = "Peperoni", Description = "Peperoni", IsAvailable = true },
            new Ingredient { Id = 6, Name = "Gamberetti", Description = "Gamberetti", IsAvailable = true },
            new Ingredient { Id = 7, Name = "Zucchine", Description = "Zucchine", IsAvailable = true },
            new Ingredient { Id = 8, Name = "Salsiccia", Description = "Salsiccia", IsAvailable = true });
        }
    }
}
