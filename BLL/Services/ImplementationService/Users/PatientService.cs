using BLL.Dtos.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.Users
{
    public class PatientService : IPatientService
    {
        public Task<PatientProfileDto> GetProfileAsync(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<PatientProfileDto> UpdateProfileAsync(int patientId, PatientProfileDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
