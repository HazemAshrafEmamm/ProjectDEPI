using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class BasketNotFoundException(string id) 
        : NotFoundException($"Basket with id : {id} Not Found")
    {
    }
}
