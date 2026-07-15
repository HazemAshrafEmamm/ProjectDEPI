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
        Task<IEnumerable<NursingRequestDto>> GetMyNursingRequestsAsync(int requesterId); 
        Task<NursingRequestDto> UpdateNursingStatusAsync(int requestId, UpdateNursingStatusDto dto, int userId); 
        Task<NursingRequestDto> CancelNursingAsync(int requestId, int userId); 
        Task<NursingReviewDto> AddNursingReviewAsync(int requestId, int patientId, CreateNursingReviewDto dto); 
        Task<IEnumerable<NursingReviewDto>?> GetNursingReviewAsync(int requestId); 
        Task<IEnumerable<NurseInfoDto>> SearchNursesAsync(SearchNurseDto searchDto);

    }
}
