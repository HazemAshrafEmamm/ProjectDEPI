using BLL.Services.AbstractServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    public class NotificationController(INotificationService _notificationService) : ApiControllerBase
    {
        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _notificationService.GetNotificationsAsync(User.GetUserId());
            return Ok(notifications);
        }
        [HttpDelete("DeleteNotification")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return NoContent();
        }
        [HttpGet("GetUnreadCount")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _notificationService.GetUnreadCountAsync(User.GetUserId());
            return Ok(count);
        }
        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return NoContent();
        }

    }
}
