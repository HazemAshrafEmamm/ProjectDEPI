namespace DAL.Exceptions
{
    public sealed class NotificationNotFoundException(int notificationId)
        : NotFoundException($"Notification with ID '{notificationId}' was not found.")
    {
    }
}
