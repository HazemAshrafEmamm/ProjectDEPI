using DAL.Data;
using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository(TabibyDbContext _context) : IUserRepository
    {
        public async Task<Patient?> GetPatientWithBasketAsync(int patientId)
        {
            return await _context
                            .Set<Patient>()
                            .Include(p => p.Basket)
                            .FirstOrDefaultAsync(p => p.Id == patientId);
        }
    }
}
