using BLL.Dtos.Consultion;
using BLL.Dtos.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.ConsultationModule
{
    public interface IConsultationService
    {
            Task<DoctorInfoDto> GetDoctorInfoAsync(int doctorId);
            Task<IEnumerable<DoctorInfoDto>> SearchDoctorsAsync(SearchDoctorDto searchDto);
            Task<ConsultationDto> GetConsultationByIdAsync(int ConsultationId , int RequesterId);
            Task<IEnumerable<ConsultationDto>> GetMyConsultationsAsync(int PatientId);
            Task<ConsultationDto> RequestConsultationAsync(int PatientId, CreateConsultationDto createDto);
            Task<ConsultationDto> UpdateConsultationStatusAsync(int consultationId, int PatientId, UpdateConsultionStatusDto updateStatusDto);
            Task DeleteConsultationAsync(int ConsultationId,int RequesterId);
    }
}
