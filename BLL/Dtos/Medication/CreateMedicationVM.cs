using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Medication
{
    public class CreateMedicationVM
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public bool is_available { get; set; }
        public int pharmacistId { get; set; }
    }
}
