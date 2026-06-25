using BLL.Dtos.Schedule;
using BLL.Services.AbstractServices.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.AppointmentModule
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        public Task<DoctorScheduleDto> CreateScheduleAsync(int doctorId, CreateDoctorScheduleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteScheduleAsync(int scheduleId, int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorScheduleDto>> GetDoctorSchedulesAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<DoctorScheduleDto> UpdateScheduleAsync(int scheduleId, UpdateDoctorScheduleDto dto, int doctorId)
        {
            throw new NotImplementedException();
        }
    }
}
