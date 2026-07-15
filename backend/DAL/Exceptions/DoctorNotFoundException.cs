using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class DoctorNotFoundException(int doctorId) 
                                  : NotFoundException($"Doctor with ID {doctorId} was not found.")
    {
    }
}
