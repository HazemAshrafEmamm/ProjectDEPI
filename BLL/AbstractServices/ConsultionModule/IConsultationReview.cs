using BLL.Dtos.Consultion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices.ConsultionModule
{
    public interface IConsultationReview
    {
            Task<ConsultationReviewDto> AddReviewAsync(string consultationId, string patientId, CreateConsultationReviewDto dto);
            Task<List<ConsultationReviewDto>> GetConsultationReviewsByConsultationId(string consultationId);
    }
}
