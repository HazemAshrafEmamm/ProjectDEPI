using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.ConsultationModule;
using DAL.Exceptions;
using DAL.Exceptions.ConsultationModule;
using DAL.Models.Consultation;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.ConsultationSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.ConsultationModule
{
    public class ConsultationReviewService(IUnitOfWork _unitOfWork ,
        IMapper _mapper , INotificationService _notificationService) : IConsultationReviewService
    {
        public async Task<ConsultationReviewDto> AddReviewAsync(int consultationId,int patientId, CreateConsultationReviewDto dto)
        {
            var consultation = await _unitOfWork.GetRepository<Consultation>()
                                                .GetByIdAsync(consultationId) ?? throw new ConsultationNotFoundException(consultationId);


            if (consultation.PatientId != patientId)
                throw new UnauthorizedAccessException("You do not have access to this consultation.");

            if (consultation.Status != ConsultationStatus.Completed)
                throw new ConsultationNotCompletedException(consultationId);

            var repo = _unitOfWork.GetRepository<ConsultationReview>();

            var existingReview = (await repo.GetAllAsync(new ReviewByConsultationIdSpec(consultationId))).FirstOrDefault();
            
            if (existingReview != null)
                throw new ConsultationReviewAlreadyExistsException(consultationId);

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
