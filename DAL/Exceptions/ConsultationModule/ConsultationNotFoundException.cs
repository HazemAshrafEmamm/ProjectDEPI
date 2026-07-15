namespace DAL.Exceptions.ConsultationModule
{
    public sealed class ConsultationNotFoundException(int consultationId)
        : NotFoundException($"Consultation with ID '{consultationId}' was not found.")
    {
    }
}
