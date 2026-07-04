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
        public async Task<Patient?> GetPatientWithAppointmentAsync(int patientId)
        {
            return await _context
                            .Set<Patient>()
                            .Include(p => p.Appointment)
                            .FirstOrDefaultAsync(p => p.Id == patientId);
        }        

        public async Task<Patient?> GetPatientWithBasketAsync(int patientId)
        {
            return await _context
                            .Set<Patient>()
                            .Include(p => p.Basket)
                            .FirstOrDefaultAsync(p => p.Id == patientId);
        }
        public async Task<Pharmacist?> GetPharmacistWithMedicationsAsync(int pharmacistId)
        {
            return await _context
                            .Set<Pharmacist>()
                            .Include(p => p.Medications)
                            .FirstOrDefaultAsync(p => p.Id == pharmacistId);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int doctorId)
        {
            return await _context
                            .Set<Doctor>()
                            .FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string? name, string? specialization, string? location, int pageNumber, int pageSize)
        {
            var query = _context.Set<Doctor>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Fullname.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(specialization))
                query = query.Where(d => d.Specialty.ToLower().Contains(specialization.ToLower()));

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(d => d.Location.ToLower().Contains(location.ToLower()));

            return await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}
