using AutoMapper;
using BLL.Dtos.Consultion;
using BLL.Hubs;
using BLL.Services.AbstractServices.ConsultationModule;
using DAL.Models.Consultation;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.ConsultationSpecs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services.ImplementationService.ConsultationModule
{
    public class ConsultationChatService (IUnitOfWork _unitOfWork , IMapper _mapper , IHubContext<ChatHub> _chatHub): IConsultationChatService
    {
        public async Task<IEnumerable<ConsultationMessageDto>> GetMessagesAsync(int consultationId, int requesterId)
        {
            var consultation = await ValidateConsultationAccessAsync(consultationId, requesterId);

            if(consultation.Status != ConsultationStatus.Accepted) 
                    throw new Exception("Consultation is not active");

            var messages = await _unitOfWork.GetRepository<ConsultationMessage>().GetAllAsync(new ConsultationMessagesByConsultationIdSpec(consultationId));

            return _mapper.Map<IEnumerable<ConsultationMessageDto>>(messages);
        }

        public async Task<int> GetUnreadCountAsync(int consultationId, int userId)
        {
             await ValidateConsultationAccessAsync(consultationId, userId);

            var messages = await _unitOfWork.GetRepository<ConsultationMessage>()
                                            .GetAllAsync(new UnReadMessagesSpecs(consultationId,userId));
            return messages.Count();
        }

        public async Task MarkMessagesAsReadAsync(int consultationId, int readerUserId)
        {
            
            await ValidateConsultationAccessAsync(consultationId,readerUserId);

            var unread = await _unitOfWork.GetRepository<ConsultationMessage>().GetAllAsync(new UnReadMessagesSpecs(consultationId, readerUserId));
            foreach (var msg in unread)
            {
                msg.IsRead = true;
                _unitOfWork.GetRepository<ConsultationMessage>().Update(msg);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ConsultationMessageDto> SendMessageAsync(int consultationId, int senderUserId, SendMessageDto dto)
        {
            var consultation = await ValidateConsultationAccessAsync(consultationId, senderUserId);
            if (consultation.Status != ConsultationStatus.Accepted)
                    throw new Exception("Consultation is not active");
            
            var message = new ConsultationMessage
            {
                ConsultationId = consultationId,
                SenderUserId = senderUserId,
                Content = dto.Content,
                IsRead = false,
                SentAt = DateTime.UtcNow
            };
            await _unitOfWork.GetRepository<ConsultationMessage>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync();
            var messageDto = _mapper.Map<ConsultationMessageDto>(message);  
            
            await _chatHub.Clients.Group($"consultation_{consultationId}")
                        .SendAsync("ReceiveMessage", messageDto);

            return messageDto;
        }


        private async Task<Consultation> ValidateConsultationAccessAsync(int consultationId,int userId)
        {
            var consultation = await _unitOfWork
                .GetRepository<Consultation>()
                .GetByIdAsync(consultationId);

            if (consultation == null)
                throw new Exception("Consultation not found");

            if (consultation.PatientId != userId &&
                consultation.DoctorId != userId)
                throw new Exception("Unauthorized");

            return consultation;
        }
    }
}
