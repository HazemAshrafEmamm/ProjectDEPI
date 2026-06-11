using DAL.Models.Consultation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.ConsultationSpecs
{
    public class MyConsultationsSpecs : BaseSpecification<Consultation>
    {
        public MyConsultationsSpecs(int requesterId) : base(c => c.PatientId == requesterId || c.DoctorId == requesterId)
        {
        }
    }
}
