namespace DAL.Exceptions.AppointmentModule
{
    public sealed class AppointmentNotCancelableException(int appointmentId)
        : BusinessRuleException($"Appointment '{appointmentId}' cannot be canceled because it is no longer in Pending status.")
    {
    }
}
