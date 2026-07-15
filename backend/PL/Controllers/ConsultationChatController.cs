using BLL.Dtos.Consultion;
using BLL.Services.AbstractServices.ConsultationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "PATIENT,DOCTOR")]
    public class ConsultationChatController(IConsultationChatService _chatService) : ApiControllerBase
    {
        [HttpGet("{consultationId}/messages")]
        public async Task<IActionResult> GetMessages(int consultationId)
        {
            var messages = await _chatService.GetMessagesAsync(consultationId, User.GetUserId());
            return Ok(messages);
        }

        [HttpPost("{consultationId}/messages")]
        public async Task<IActionResult> SendMessage(int consultationId, [FromBody] SendMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = await _chatService.SendMessageAsync(consultationId, User.GetUserId(), dto);
            return Ok(message);
        }

        [HttpPost("{consultationId}/read")]
        public async Task<IActionResult> MarkAsRead(int consultationId)
        {
            await _chatService.MarkMessagesAsReadAsync(consultationId, User.GetUserId());
            return NoContent();
        }

        [HttpGet("{consultationId}/unread-count")]
        public async Task<IActionResult> GetUnreadCount(int consultationId)
        {
            var count = await _chatService.GetUnreadCountAsync(consultationId, User.GetUserId());
            return Ok(count);
        }
    }
}
