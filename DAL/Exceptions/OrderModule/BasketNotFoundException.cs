using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions.OrderModule
{
    public class BasketNotFoundException(int patientId) 
        : NotFoundException($"Basket with Patient id : {patientId} Not Found")
    {
    }
}
