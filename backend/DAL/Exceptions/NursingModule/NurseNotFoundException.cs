namespace DAL.Exceptions.NursingModule
{
    public sealed class NurseNotFoundException(int nurseId)
        : NotFoundException($"Nurse with ID '{nurseId}' was not found.")
    {
    }
}
