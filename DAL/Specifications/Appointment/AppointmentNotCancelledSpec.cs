using DAL.Models.Users;
using DAL.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AppointmentEntity = DAL.Models.AppointmentModule.Appointment;

namespace DAL.Specifications.Appointment
{
    public class AppointmentNotCancelledSpec:BaseSpecification<AppointmentEntity>
    {
        public AppointmentNotCancelledSpec(int doctorId, DateTime date) :base(a => a.DoctorId == doctorId
                 && a.AppointmentDate.Date == date.Date
                 && a.Status != AppointmentStatus.Cancelled)
        {
            
        }
    }
}
