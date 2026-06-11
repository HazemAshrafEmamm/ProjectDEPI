using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Shared;

namespace DAL.Models.Users
{
    public class Patient : ApplicationUser
    {
        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
