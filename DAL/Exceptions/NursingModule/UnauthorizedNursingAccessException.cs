namespace DAL.Exceptions.NursingModule
{
    public sealed class UnauthorizedNursingAccessException(int userId, int requestId)
        : UnauthorizedAccessException($"User '{userId}' is not authorized to modify nursing request '{requestId}'.")
    {
    }
}
