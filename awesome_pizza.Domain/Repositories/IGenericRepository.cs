using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
