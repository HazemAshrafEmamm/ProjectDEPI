namespace DAL.Exceptions.AppointmentModule
{
    public sealed class UnauthorizedAppointmentAccessException(int userId, int appointmentId)
        : UnauthorizedAccessException($"User '{userId}' is not authorized to access appointment '{appointmentId}'.")
    {
    }
}
