namespace DAL.Exceptions.ConsultationModule
{
    public sealed class ConsultationNotCompletedException(int consultationId)
        : BusinessRuleException($"Consultation '{consultationId}' is not completed yet. Reviews can only be added after completion.")
    {
    }
}
