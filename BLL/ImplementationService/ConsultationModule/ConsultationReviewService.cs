using AutoMapper;
using BLL.AbstractServices;
using BLL.AbstractServices.ConsultionModule;
using BLL.Dtos.Consultion;
using DAL.Models.Consultation;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.ConsultationSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService.ConsultationModule
{
    public class ConsultationReviewService(IUnitOfWork _unitOfWork ,
        IMapper _mapper , INotificationService _notificationService) : IConsultationReviewService
    {
        public async Task<ConsultationReviewDto> AddReviewAsync(int consultationId,int patientId, CreateConsultationReviewDto dto)
        {
            var consultation = await _unitOfWork.GetRepository<Consultation>()
                                                .GetByIdAsync(consultationId);

            if (consultation == null || consultation.PatientId != patientId)
                throw new Exception("Consultation not found or access denied.");
            
            if(consultation.Status != ConsultationStatus.Completed)
                throw new Exception("Consultation is not completed yet.");

            var repo = _unitOfWork.GetRepository<ConsultationReview>();

            var existingReview = (await repo.GetAllAsync(new ReviewByConsultationIdSpec(consultationId))).FirstOrDefault();
            
            if (existingReview != null)
                throw new Exception("A review for this consultation already exists.");

            var review = _mapper.Map<ConsultationReview>(dto);
            
            review.ConsultationId = consultationId;

            await repo.AddAsync(review);
            await _unitOfWork.SaveChangesAsync(); 

            await _notificationService.SendNotificationAsync( $"New review added for consultation {consultationId}.", NotificationType.ConsultationReview, consultation.DoctorId);
            return _mapper.Map<ConsultationReviewDto>(review) ;
        }

        public async Task<ConsultationReviewDto?> GetConsultationReviewsByConsultationId(int consultationId)
        {
            var repo = _unitOfWork.GetRepository<ConsultationReview>();

            var review = (await repo.GetAllAsync(new ReviewByConsultationIdSpec(consultationId))).FirstOrDefault();

            if (review is null)
                return null;

            return _mapper.Map<ConsultationReviewDto>(review);
        }
    }
}
