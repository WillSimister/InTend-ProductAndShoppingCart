using System;

namespace InTend_ProductAndShoppingCart.Business.Exceptions
{
    public class ItemOutOfStockException : Exception
    {
        public Guid ProductId { get; }

        public ItemOutOfStockException(Guid productId, int currentStockLevel, int requestedStock)
            : base($"Product with ID '{productId}' does not have enough stock to complete request. Current Stock Level: {currentStockLevel}, Requested Stock: {requestedStock}")
        {
            ProductId = productId;
        }
    }
}
