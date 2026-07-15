namespace DAL.Exceptions.OrderModule
{
    public sealed class BasketAlreadyCheckedOutException(int basketId)
        : ConflictException($"Basket '{basketId}' is already checked out. An order was already created from it.")
    {
    }
}
