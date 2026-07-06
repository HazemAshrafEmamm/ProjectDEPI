using DAL.Models.NursingModule;

namespace DAL.Specifications.NursingSpecs
{
    public class NursingRequestsByPatientIdSpec : BaseSpecification<NursingRequest>
    {
        public NursingRequestsByPatientIdSpec(int patientId)
            : base(r => r.PatientId == patientId)
        {
            AddInclude(r => r.Review);
        }
    }
}