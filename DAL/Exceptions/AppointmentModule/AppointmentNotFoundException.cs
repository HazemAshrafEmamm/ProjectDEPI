namespace DAL.Exceptions.AppointmentModule
{
    public sealed class AppointmentNotFoundException(int appointmentId)
        : NotFoundException($"Appointment with ID '{appointmentId}' was not found.")
    {
    }
}
