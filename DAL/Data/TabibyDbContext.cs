using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Data
{
    public class TabibyDbContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }
        
        public TabibyDbContext(DbContextOptions<TabibyDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TabibyDbContext).Assembly);
        }
    }
}
