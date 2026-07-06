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
        Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int requesterId); // For patients to view their own requests
        Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId); 
        Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId); 
        Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto); // For patients to review completed nursing services
        Task<IEnumerable<NursingReviewDto>?> GetNursingReviewAsync(int requestId); //for nurses to view reviews of their reviewed services
        Task<IEnumerable<NursingReviewDto>?> GetNursingReviewByNurseIdAsync(int nurseId); //for patients to view reviews of a specific nurse
        Task<IEnumerable<NurseInfoDto>> SearchNursesAsync(SearchNurseDto searchDto);

    }
}
