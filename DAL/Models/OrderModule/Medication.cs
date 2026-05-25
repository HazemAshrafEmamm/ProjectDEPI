using DAL.Models.Users;
using DAL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.OrderModule
{
    public class Medication : BaseEntity
    {

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Is_available { get; set; }
        

        /*Navigation Properties*/
        public virtual List<Order_Item> Order_Item { get; set; }
        public string PharmacistId { get; set; }

        public virtual Pharmacist Pharmacist { get; set; }
    }
}
