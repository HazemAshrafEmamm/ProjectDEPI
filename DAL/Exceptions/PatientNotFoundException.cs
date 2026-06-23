namespace DAL.Exceptions
{
    public sealed class PatientNotFoundException(int patientId)
        : NotFoundException($"Patient with ID '{patientId}' was not found.")
    {
    }
}
