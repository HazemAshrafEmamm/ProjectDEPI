using System;
using DAL.Models.Users;
using DAL.Shared;
using DAL.Shared.Enums;

namespace DAL.Models.AppointmentModule
{
    public class Appointment : BaseEntity
    {
        public string PatientId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public int ScheduleId { get; set; }
        
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }

        public DoctorSchedule Schedule { get; set; } = null!;

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
