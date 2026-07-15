using DAL.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions.OrderModule
{
    public class OrderCantBeDeletedException(int OrderId) :
         Exception($"Order '{OrderId}' cannot be Deleted ")
    {
    }
}
