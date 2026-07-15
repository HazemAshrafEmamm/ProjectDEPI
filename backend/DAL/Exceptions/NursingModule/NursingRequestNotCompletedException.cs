namespace DAL.Exceptions.NursingModule
{
    public sealed class NursingRequestNotCompletedException(int requestId)
        : BusinessRuleException($"Nursing request '{requestId}' is not completed yet. Reviews can only be added after completion.")
    {
    }
}
