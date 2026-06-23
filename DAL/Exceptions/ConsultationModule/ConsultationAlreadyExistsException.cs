namespace DAL.Exceptions.ConsultationModule
{
    public sealed class ConsultationAlreadyExistsException(int patientId, int doctorId)
        : ConflictException($"Patient '{patientId}' already has a pending consultation with doctor '{doctorId}'.")
    {
    }
}
