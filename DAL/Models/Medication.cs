using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public String? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public bool? Is_available { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;

        /*Navigation Properties*/
        public virtual List<Order_Item> Order_Item { get; set; }

        public int? PharmacistId { get; set; }
        //public virtual Pharmacist Pharmacist { get; set; }
    }
}