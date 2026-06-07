using DAL.Models;

namespace DAL.Specifications.NotificationSpecs
{
    public class UnreadNotificationsForUserSpecification : BaseSpecification<Notification>
    {
        public UnreadNotificationsForUserSpecification(string userId)
            : base(n => n.UserId == userId && !n.IsRead)
        {
        }
    }
}
