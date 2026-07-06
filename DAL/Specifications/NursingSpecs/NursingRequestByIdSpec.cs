using DAL.Models.NursingModule;

namespace DAL.Specifications.NursingSpecs
{
    public class NursingRequestByIdSpec : BaseSpecification<NursingRequest>
    {
        public NursingRequestByIdSpec(int requestId)
            : base(r => r.Id == requestId)
        {
            AddInclude(r => r.Review);
        }
    }
}