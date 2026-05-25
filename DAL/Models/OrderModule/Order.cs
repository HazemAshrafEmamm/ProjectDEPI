using DAL.Models.Users;
using DAL.Shared;
using DAL.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.OrderModule
{
    public class Order : BaseEntity
    {
        public DateTime Order_date { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }

        /*Navigation Properties*/

        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual List<Order_Item> Order_Item { get; set; }

    }
}
