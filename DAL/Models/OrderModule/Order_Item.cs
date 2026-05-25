using DAL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.OrderModule
{
    public class Order_Item : BaseEntity
    {
        public int quantity { get; set; }
        public decimal unit_price { get; set; }


        /*Navigation Properties*/

        public string MedicationId { get; set; }
        public virtual Medication Medication { get; set; }

        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
