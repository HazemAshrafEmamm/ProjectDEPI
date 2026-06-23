namespace DAL.Exceptions.OrderModule
{
    public sealed class OrderNotFoundException(int orderId)
        : NotFoundException($"Order with ID '{orderId}' was not found.")
    {
    }
}
