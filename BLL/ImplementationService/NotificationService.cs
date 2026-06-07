using BLL.AbstractServices;
using BLL.Dtos;
using BLL.Hubs;
using DAL.Models;
using DAL.Repository;
using DAL.Shared.Enums;
using DAL.Specifications.NotificationSpecs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ImplementationService
{
    public class NotificationService(IUnitOfWork _unitOfWork , IHubContext<NotificationHub> hubContext) : INotificationService
    {
        public async Task DeleteNotificationAsync(string notificationId)
        {
            var notification = await _unitOfWork.GetRepository<Notification>().GetByIdAsync(notificationId);
            if (notification == null)
                throw new Exception("Notification not found");
            _unitOfWork.GetRepository<Notification>().Delete(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<string>> GetNotificationsAsync(string UserId)
        {
            var notifications = await _unitOfWork.GetRepository<Notification>().GetAllAsync(new NotificationsByUserIdSpecs(UserId));
            return notifications.Select(n => n.Message).ToList();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            var spec = new UnreadNotificationsForUserSpecification(userId);

            var unreadNotifications = await _unitOfWork.GetRepository<Notification>().GetAllAsync(spec);
            return unreadNotifications.Count();
        }

        public async Task MarkAsReadAsync(string notificationId)
        {
            var notification = await _unitOfWork.GetRepository<Notification>().GetByIdAsync(notificationId);
            if (notification == null)
                throw new Exception("Notification not found");
            notification.IsRead = true;
            _unitOfWork.GetRepository<Notification>().Update(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendNotificationAsync(string message, string Type, string UserId)
        {
            var notification = new Notification
            {
                UserId = UserId,
                Message = message,
                Type = Enum.Parse<NotificationType>(Type),
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.GetRepository<Notification>().AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();


            await hubContext.Clients.User(UserId).SendAsync("NewNotification", new NotificationDto
            {
                Id = notification.Id,
                Message = message,
                IsRead = false,
                CreatedAt = notification.CreatedAt
            });
        }
    }
}
