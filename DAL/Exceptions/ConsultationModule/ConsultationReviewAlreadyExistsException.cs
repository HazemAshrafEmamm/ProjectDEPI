namespace DAL.Exceptions.ConsultationModule
{
    public sealed class ConsultationReviewAlreadyExistsException(int consultationId)
        : ConflictException($"A review for consultation '{consultationId}' already exists.")
    {
    }
}
