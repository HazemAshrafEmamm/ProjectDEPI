using DAL.Models.NursingModule;

namespace DAL.Specifications.NursingRequestSpecs
{
    public class NursingReviewByRequestIdSpec : BaseSpecification<NursingReview>
    {
        public NursingReviewByRequestIdSpec(int requestId)
            : base(r => r.NursingRequestId == requestId)
        {
        }
    }
}