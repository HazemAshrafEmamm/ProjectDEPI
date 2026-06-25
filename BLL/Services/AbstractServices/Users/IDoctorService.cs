using BLL.Dtos.Appointment;
using BLL.Dtos.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.Users
{
    public interface IDoctorService
    {
        Task<DoctorInfoDto> GetDoctorInfoAsync(int doctorId);
        Task<IEnumerable<DoctorInfoDto>> SearchDoctorsAsync(SearchDoctorDto searchDto);
        Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId);
    }
}
