

using BLL.Dtos.Nursing;

namespace BLL.Services.AbstractServices.Users
{
    public class NursingService : INursingService
    {
        public Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NursingRequestDto>> GetNurseRequestsAsync(int nurseId)
        {
            throw new NotImplementedException();
        }

        public Task<NursingReviewDto?> GetNursingReviewAsync(int requestId)
        {
            throw new NotImplementedException();
        }

        public Task<NursingRequestDto> RequestNursingAsync(int patientId, CreateNursingRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
