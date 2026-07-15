namespace DAL.Exceptions.NursingModule
{
    public sealed class NursingRequestNotCancelableException(int requestId)
        : BusinessRuleException($"Nursing request '{requestId}' cannot be canceled because it is no longer in Pending status.")
    {
    }
}
