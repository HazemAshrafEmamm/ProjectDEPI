using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Order
{
    public class CreateOrderDto
    {
        public int BasketId { get; set; }
        
        public OrderAddressDto Address { get; set; }
    }
}
