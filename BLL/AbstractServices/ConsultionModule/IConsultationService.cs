using BLL.Dtos.Consultion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices.ConsultionModule
{
    public interface IConsultationService
    {
            Task<ConsultationDto> GetConsultationByIdAsync(string Id);
            Task<IEnumerable<ConsultationDto>> GetMyConsultationsAsync(string userId, string role);
            Task<ConsultationDto> CreateConsultationAsync(CreateConsultationDto createDto);
            Task UpdateConsultationStatusAsync(string Id, UpdateConsultionStatusDto updateStatusDto);
            Task DeleteConsultationAsync(string Id);
    }
}
