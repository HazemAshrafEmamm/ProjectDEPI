using BLL.AbstractServices.ConsultionModule;
using BLL.Dtos.Consultion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService.ConsultationModule
{
    public class ConsultationChatService : IConsultationChatService
    {
        public Task<IEnumerable<ConsultationMessageDto>> GetMessagesAsync(int consultationId, int requesterId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUnreadCountAsync(int consultationId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ConsultationMessageDto> SendMessageAsync(int consultationId, int senderUserId, SendMessageDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
