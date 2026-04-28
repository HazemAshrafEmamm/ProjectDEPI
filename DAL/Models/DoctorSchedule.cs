using System;
using System.Collections.Generic;
using DAL.Shared;

namespace DAL.Models
{
    public class DoctorSchedule : BaseEntity
    {
        public string DoctorId { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
