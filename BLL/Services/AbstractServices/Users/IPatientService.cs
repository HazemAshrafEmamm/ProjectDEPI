using BLL.Dtos.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.Users
{
    public interface IPatientService
    {
        Task<PatientProfileDto> GetProfileAsync(int patientId);
        Task<PatientProfileDto> UpdateProfileAsync(int patientId, PatientProfileDto dto);
    }
}
