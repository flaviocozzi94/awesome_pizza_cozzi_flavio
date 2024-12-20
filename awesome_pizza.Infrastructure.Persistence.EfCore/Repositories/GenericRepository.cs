using awesome_pizza.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Infrastructure.Persistence.EfCore.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AwesomePizzaDbContext context;

        public GenericRepository(AwesomePizzaDbContext context)
        {
            this.context = context;
        }
        public T Create(T entity)
        {
            return context.Set<T>().Add(entity).Entity;
        }

        public T Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return entity;
        }

        public T Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
