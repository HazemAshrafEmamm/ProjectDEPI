using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications.ConsultationSpecs
{
    public class ConsultationMessagesByConsultationIdSpec : BaseSpecification<ConsultationMessage>
    {
        public ConsultationMessagesByConsultationIdSpec(int consultationId) : base(m => m.ConsultationId == consultationId)
        {
            AddInclude(m => m.Sender);
            ApplyOrderBy(m => m.SentAt);
            
        }
    }
}
