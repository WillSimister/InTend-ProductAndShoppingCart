namespace InTend_ProductAndShoppingCart.Business.Validation
{
    internal static class ProductInputValidator
    {
        internal static void ValidateId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be an empty GUID.");
            }
        }

        internal static void ValidateQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }
        }
    }
}
