using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Order_date { get; set; }
        public String? Status { get; set; }
        public decimal? Total { get; set; }
        public DateTime created_at { get; set; }=DateTime.Now;

        /*Navigation Properties*/

        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public virtual List<Order_Item> Order_Item { get; set; }

    }
}
