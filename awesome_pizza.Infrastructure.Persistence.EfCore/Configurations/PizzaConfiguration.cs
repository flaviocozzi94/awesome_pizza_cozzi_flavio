using awesome_pizza.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Configurations
{
    public class PizzaConfiguration : IEntityTypeConfiguration<Pizza>
    {
        public void Configure(EntityTypeBuilder<Pizza> builder)
        {
            builder.ToTable(nameof(Pizza));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(x => x.InsertDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            //builder.HasData(
            //    new Pizza { Description = "Margherita", Id = 1, IsAvailable = true, Name = "Margherita", Price = 5 },
            //    new Pizza { Description = "Marinara", Id = 2, IsAvailable = true, Name = "Marinara", Price = 5 },
            //    new Pizza { Description = "Prosciutto e funghi", Id = 3, IsAvailable = true, Name = "Prosciutto e funghi", Price = 5 },
            //    new Pizza { Description = "Salsiccia e peperoni", Id = 4, IsAvailable = true, Name = "Salsiccia e peperoni", Price = 5 },
            //    new Pizza { Description = "Zucchine e gamberetti", Id = 5, IsAvailable = true, Name = "Zucchine e gamberetti", Price = 5 });
        }
    }
}
