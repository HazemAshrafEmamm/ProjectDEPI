using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IUserRepository
    {
        Task<Patient?> GetPatientWithBasketAsync(int patientId);
        Task<Patient?> GetPatientWithAppointmentAsync(int patientId);
        Task<Pharmacist?> GetPharmacistWithMedicationsAsync(int pharmacistId);
        Task<Doctor?> GetDoctorByIdAsync(int doctorId);
        Task<IEnumerable<Doctor>> SearchDoctorsAsync(string? name, string? specialization, string? location, int pageNumber, int pageSize);
        Task<IEnumerable<Nurse>> SearchNursesAsync(string? name, string? specialization, int pageNumber, int pageSize);
    }
}
