namespace DAL.Exceptions.ConsultationModule
{
    public sealed class ConsultationNotActiveException(int consultationId)
        : BusinessRuleException($"Consultation '{consultationId}' is not active. Messages can only be sent in accepted consultations.")
    {
    }
}
