namespace DAL.Exceptions.NursingModule
{
    public sealed class NursingReviewAlreadyExistsException(int requestId)
        : ConflictException($"A review for nursing request '{requestId}' already exists.")
    {
    }
}
