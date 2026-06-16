using DAL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.BasketModule
{
    public class CustomerBasket : BaseEntity
    {
        public IEnumerable<BasketItems> BasketItems { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
