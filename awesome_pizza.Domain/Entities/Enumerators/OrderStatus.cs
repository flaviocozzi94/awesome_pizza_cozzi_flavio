using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Domain.Entities.Enumerators
{
    public enum OrderStatus
    {
        Open = 1,
        Preparing = 2,
        Delivering = 3,
        Delivered = 4,
        Deleted = 5
    }
}
