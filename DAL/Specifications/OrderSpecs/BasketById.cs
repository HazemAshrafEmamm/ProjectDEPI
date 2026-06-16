using DomainLayer.Models.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.OrderSpecs
{
    public class BasketById : BaseSpecification<CustomerBasket>
    {
        public BasketById(int basketId) : base(b => b.Id == basketId)
        {
            AddInclude(b => b.BasketItems);
        }
    }
}
