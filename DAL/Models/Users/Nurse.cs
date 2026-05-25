using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;
using DAL.Models.NursingModule;
using DAL.Shared;

namespace DAL.Models.Users
{
    public class Nurse : BaseEntity
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public virtual ICollection<NursingRequest> NursingRequests { get; set; } = new List<NursingRequest>();
        
    }
}
