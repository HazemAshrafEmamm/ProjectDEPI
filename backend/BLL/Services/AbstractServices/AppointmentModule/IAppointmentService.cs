using BLL.Dtos.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.AppointmentModule
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> GetAppointmentAsync(int appointmentId, int userId);
        Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(int userId);
        Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId);
        Task<AppointmentDto> BookAppointmentAsync(int patientId, CreateAppointmentDto dto);
        Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, int userId);
        Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId, int doctorId);
        Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId, int doctorId);
        Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto, int userId);
        Task<IEnumerable<AvailableDoctorSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date);
    }
}
