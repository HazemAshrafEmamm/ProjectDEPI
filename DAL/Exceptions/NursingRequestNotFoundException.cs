namespace DAL.Exceptions
{
    public sealed class NursingRequestNotFoundException(int id)
        : NotFoundException($"Nursing request with ID '{id}' was not found.")
    {
    }
}