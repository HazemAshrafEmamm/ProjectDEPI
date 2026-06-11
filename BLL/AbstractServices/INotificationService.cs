using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface INotificationService
    {
        public Task SendNotificationAsync(string message, string Type , int UserId);

        public Task<List<string>> GetNotificationsAsync(int UserId);
        Task MarkAsReadAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);
        Task<int> GetUnreadCountAsync(int userId);

    }
}
