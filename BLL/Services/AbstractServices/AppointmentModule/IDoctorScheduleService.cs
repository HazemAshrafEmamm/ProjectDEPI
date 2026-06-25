using BLL.Dtos.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.AppointmentModule
{
    public interface IDoctorScheduleService
    {
        Task<DoctorScheduleDto> CreateScheduleAsync(int doctorId, CreateDoctorScheduleDto dto);
        Task<IEnumerable<DoctorScheduleDto>> GetDoctorSchedulesAsync(int doctorId);
        Task<DoctorScheduleDto> UpdateScheduleAsync(int scheduleId, UpdateDoctorScheduleDto dto, int doctorId);
        Task<bool> DeleteScheduleAsync(int scheduleId, int doctorId);
    }
}
