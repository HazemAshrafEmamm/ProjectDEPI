using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL.Data
{
    public class TabibyDbContext : IdentityDbContext
    {
        
        
        public TabibyDbContext(DbContextOptions<TabibyDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TabibyDbContext).Assembly);
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

    }
}
