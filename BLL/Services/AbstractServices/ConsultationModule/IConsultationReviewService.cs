using BLL.Dtos.Consultion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.ConsultationModule
{
    public interface IConsultationReviewService
    {
            Task<ConsultationReviewDto> AddReviewAsync(int consultationId, int patientId, CreateConsultationReviewDto dto);
            Task<ConsultationReviewDto?> GetConsultationReviewsByConsultationId(int consultationId);
    }
}
