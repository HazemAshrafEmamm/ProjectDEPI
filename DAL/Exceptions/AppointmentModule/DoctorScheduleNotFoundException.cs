namespace DAL.Exceptions.AppointmentModule
{
    public sealed class DoctorScheduleNotFoundException(int scheduleId)
        : NotFoundException($"Doctor schedule with ID '{scheduleId}' was not found.")
    {
    }
}
