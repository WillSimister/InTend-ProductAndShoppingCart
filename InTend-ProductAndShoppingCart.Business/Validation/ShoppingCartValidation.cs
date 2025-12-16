using InTend_ProductAndShoppingCart.Business.Exceptions;

namespace InTend_ProductAndShoppingCart.Business.Validation
{
    internal class ShoppingCartValidation
    {
        internal static bool ItemHasEnoughStock(Guid productGuid, int quantityRequested, int quantityInStock)
        {
            if (quantityRequested <= quantityInStock)
            {
                return true;
            }
            else throw new ItemOutOfStockException(productGuid, quantityInStock, quantityRequested);

        }
    }
}
