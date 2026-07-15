namespace DAL.Exceptions.OrderModule
{
    public sealed class InsufficientStockException(string medicationName, int availableStock)
        : BusinessRuleException($"'{medicationName}' has insufficient stock. Available quantity: {availableStock}.")
    {
    }
}
