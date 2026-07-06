namespace DAL.Exceptions.AppointmentModule
{
    public sealed class AppointmentSlotUnavailableException(int doctorId, DateTime date, TimeSpan time)
        : ConflictException($"The appointment slot at {time:hh\\:mm} on {date:yyyy-MM-dd} with doctor '{doctorId}' is not available.")
    {
    }
}
