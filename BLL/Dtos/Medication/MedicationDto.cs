using DAL.Models.OrderModule;
using DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Medication
{
    public class MedicationDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Is_available { get; set; }
        public string PharmacistId { get; set; }

    }
}
