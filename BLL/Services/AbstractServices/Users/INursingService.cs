using BLL.Dtos.Nursing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.Users
{
    public interface INursingService
    {
        Task<NursingRequestDto> RequestNursingAsync(int patientId, CreateNursingRequestDto dto);
        Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int patientId); // For patients to view their own requests
        Task<IEnumerable<NursingRequestDto>> GetNurseRequestsAsync(int nurseId); // For nurses to view requests assigned to them
        Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId); 
        Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId); 
        Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto); // For patients to review completed nursing services
        Task<NursingReviewDto?> GetNursingReviewAsync(int requestId); 
    }
}
