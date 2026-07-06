using DAL.Models.NursingModule;

namespace DAL.Specifications.NursingSpecs
{
    public class NursingReviewByRequestIdSpec : BaseSpecification<NursingReview>
    {
        public NursingReviewByRequestIdSpec(int requestId)
            : base(r => r.NursingRequestId == requestId)
        {
        }
    }
}