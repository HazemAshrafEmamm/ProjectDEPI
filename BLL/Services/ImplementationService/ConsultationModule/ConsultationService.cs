using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Services.AbstractServices;
using BLL.Services.AbstractServices.ConsultationModule;
using DAL.Models.Consultation;
using DAL.Models.Users;
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
    public class ConsultationService
        (IUnitOfWork _unitOfWork , IMapper _mapper , INotificationService _notificationService) : IConsultationService
    {
        public async Task<ConsultationDto> RequestConsultationAsync(int PatientId, CreateConsultationDto createDto)
        {
            var existingConsultation = (await _unitOfWork.GetRepository<Consultation>()
                                            .GetAllAsync(new AllowedConsultationSpecs(PatientId, createDto.DoctorId))).FirstOrDefault();
            if (existingConsultation is not null )
                 throw new InvalidOperationException("You already have a pending consultation request.");

            var consultation = new Consultation
            {
                PatientId = PatientId,
                DoctorId = createDto.DoctorId,
                Status = ConsultationStatus.Pending,
            };

            await _unitOfWork.GetRepository<Consultation>().AddAsync(consultation);
            await _unitOfWork.SaveChangesAsync();
            
            await _notificationService.SendNotificationAsync( $"You have a new consultation request from patient {PatientId}." ,NotificationType.ConsultationRequest, createDto.DoctorId);
    
            return _mapper.Map<ConsultationDto>(consultation);
        }

        public async Task DeleteConsultationAsync(int ConsultationId, int RequesterId)
        {
            var Consultation = (await _unitOfWork.GetRepository<Consultation>()
                                .GetAllAsync(new ConsultationByIdSpecs(ConsultationId, RequesterId))).FirstOrDefault();
            if (Consultation is null)
                throw new KeyNotFoundException("Consultation not found.");
             _unitOfWork.GetRepository<Consultation>().Delete(Consultation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ConsultationDto> GetConsultationByIdAsync(int ConsultationId, int RequesterId)
        {
            var consultation = (await _unitOfWork.GetRepository<Consultation>()
                                          .GetAllAsync(new ConsultationByIdSpecs(ConsultationId, RequesterId))).FirstOrDefault();
            if (consultation is null)
                throw new KeyNotFoundException("Consultation not found or you don't have access to it.");
            return _mapper.Map<ConsultationDto>(consultation);
        }

        public async Task<IEnumerable<ConsultationDto>> GetMyConsultationsAsync(int RequesterId)
        {
            var consultations = await _unitOfWork.GetRepository<Consultation>()
                                        .GetAllAsync(new MyConsultationsSpecs(RequesterId));

            if (!consultations.Any())
                return Enumerable.Empty<ConsultationDto>();

            return _mapper.Map<IEnumerable<ConsultationDto>>(consultations);
        }

        public async Task<ConsultationDto> UpdateConsultationStatusAsync(int consultationId, int DoctorId, UpdateConsultionStatusDto updateStatusDto)
        {
            var consultation = await _unitOfWork.GetRepository<Consultation>().GetByIdAsync(consultationId)
                ?? throw new KeyNotFoundException("Consultation not found.");

            if (consultation.DoctorId != DoctorId)
                throw new UnauthorizedAccessException("Only the assigned doctor can update status.");

            if (!Enum.TryParse(updateStatusDto.status, out ConsultationStatus status))
                throw new ArgumentException("Invalid status");

            consultation.Status = status;

            _unitOfWork.GetRepository<Consultation>().Update(consultation);

            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync( $"Your consultation request has been {status.ToString().ToLower()} by doctor {DoctorId}." ,NotificationType.ConsultationUpdate, consultation.PatientId);

            return _mapper.Map<ConsultationDto>(consultation);
        }
    }
}
