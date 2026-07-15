namespace DAL.Exceptions.AppointmentModule
{
    public sealed class DoctorScheduleConflictException(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        : ConflictException($"A schedule already exists for {day} from {startTime:hh\\:mm} to {endTime:hh\\:mm}.")
    {
    }
}
