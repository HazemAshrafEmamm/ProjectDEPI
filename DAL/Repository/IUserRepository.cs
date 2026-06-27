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



    }
}
