using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Shared;

namespace DAL.Models.Users
{
    public class Patient : BaseEntity
    {
        public string UserId { get; set; } // forign key 

        public ApplicationUser User { get; set; } // navigation property 

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
