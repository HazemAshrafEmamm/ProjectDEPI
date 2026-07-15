using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions.AppointmentModule
{
    public class AppointmentNotConfirmableException : BusinessRuleException
    {
        public AppointmentNotConfirmableException(int appointmentId)
            : base($"Appointment '{appointmentId}' cannot be confirmed.")
        {
        }
    }
}
