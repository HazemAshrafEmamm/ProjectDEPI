using DAL.Models.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.Appointment
{
    public class GetAvailableSlotsSpecs:BaseSpecification<DoctorSchedule>
    {
        public GetAvailableSlotsSpecs(int doctorId, DateTime date)
       : base(a => a.DoctorId == doctorId && a.DayOfWeek==date.DayOfWeek)
        {

        }
    }
}
