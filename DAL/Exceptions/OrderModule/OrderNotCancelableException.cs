namespace DAL.Exceptions.OrderModule
{
    public sealed class OrderNotCancelableException(int orderId)
        : BusinessRuleException($"Order '{orderId}' cannot be canceled because it is no longer in Pending status.")
    {
    }
}
