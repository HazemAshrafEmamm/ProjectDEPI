using DAL.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.OrderSpecs
{
    public class OrdersByPatientId : BaseSpecification<Order>
    {
        public OrdersByPatientId(int patientId) : base(o => o.PatientId == patientId)
        {
            AddInclude(o => o.Order_Item);
            AddInclude(o => o.Address);
        }
    }
}
