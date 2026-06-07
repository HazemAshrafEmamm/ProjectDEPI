using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface INotificationService
    {
        public Task SendNotificationAsync(string message, string Type , string UserId);

        public Task<List<string>> GetNotificationsAsync(string UserId);
        Task MarkAsReadAsync(string notificationId);
        Task DeleteNotificationAsync(string notificationId);
        Task<int> GetUnreadCountAsync(string userId);

    }
}
