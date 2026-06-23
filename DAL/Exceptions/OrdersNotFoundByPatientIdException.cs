using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public sealed class OrdersNotFoundByPatientIdException(int patientId) 
               : NotFoundException($"Orders for Patient with ID: '{patientId}' were not found.")
    {
    }
}
