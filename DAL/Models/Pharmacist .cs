//using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;


namespace DAL.Models
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public string PharmacyName { get; set; } = string.Empty;

        public virtual ICollection<Medication> Medications { get; set; } = new List<Medication>();
    }
}