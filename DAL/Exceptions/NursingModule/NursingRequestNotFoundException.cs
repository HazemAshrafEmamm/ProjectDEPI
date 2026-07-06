namespace DAL.Exceptions.NursingModule
{
    public sealed class NursingRequestNotFoundException(int requestId)
        : NotFoundException($"Nursing request with ID '{requestId}' was not found.")
    {
    }
}
