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
    public class OrderPizzaConfiguration : IEntityTypeConfiguration<OrderPizza>
    {
        public void Configure(EntityTypeBuilder<OrderPizza> builder)
        {
            builder.ToTable(nameof(OrderPizza));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderPizzas)
                .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Pizza)
                .WithMany(x => x.OrderPizzas)
                .HasForeignKey(x => x.PizzaId);

        }
    }
}
