using DAL.Models.NursingModule;

namespace DAL.Specifications.NursingSpecs
{
    public class NursingRequestsByNurseIdSpec : BaseSpecification<NursingRequest>
    {
        public NursingRequestsByNurseIdSpec(int nurseId)
            : base(r => r.NurseId == nurseId)
        {
            AddInclude(r => r.Review);
        }
    }
}