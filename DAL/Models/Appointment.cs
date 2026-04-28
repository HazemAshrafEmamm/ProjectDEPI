using System;
using DAL.Shared;

namespace DAL.Models
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

        public ApplicationUser? Patient { get; set; }
        public ApplicationUser? Doctor { get; set; }
    }
}
