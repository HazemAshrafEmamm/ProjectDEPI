using BLL.Dtos.Appointment;
using BLL.Services.AbstractServices.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.AppointmentModule
{
    public class AppointmentService : IAppointmentService
    {
        public Task<AppointmentDto> BookAppointmentAsync(int patientId, CreateAppointmentDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> GetAppointmentAsync(int appointmentId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
