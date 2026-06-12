using BLL.Dtos.Consultion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AbstractServices.ConsultationModule
{
    public interface IConsultationChatService
    {
        Task<IEnumerable<ConsultationMessageDto>> GetMessagesAsync(int consultationId, int requesterId);
        Task<ConsultationMessageDto> SendMessageAsync(int consultationId, int senderUserId, SendMessageDto dto);
        Task MarkMessagesAsReadAsync(int consultationId, int readerUserId);

        Task<int> GetUnreadCountAsync(int consultationId, int userId);
    }
}
