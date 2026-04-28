using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order_Item
    {
        public int Id { get; set; }
        public int ?quantity { get; set; }
        public decimal? unit_price { get; set; }


        /*Navigation Properties*/

        public int? MedicationId { get; set; }
        public virtual Medication Medication { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
