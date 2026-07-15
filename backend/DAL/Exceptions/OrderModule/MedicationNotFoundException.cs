namespace DAL.Exceptions.OrderModule
{
    public sealed class MedicationNotFoundException(int medicationId)
        : NotFoundException($"Medication with ID '{medicationId}' was not found.")
    {
    }
}
