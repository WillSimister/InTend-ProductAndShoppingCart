using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Data.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        // Singleton instance
        private static readonly Lazy<ShoppingCartRepository> _instance = new(() => new ShoppingCartRepository());

        // Public accessor for the singleton instance
        public static ShoppingCartRepository Instance => _instance.Value;

        public ShoppingCartRepository() { }

        private readonly Dictionary<Guid, int> ShoppingCart = new();
        
        public IReadOnlyDictionary<Guid, int> GetCartContents()
        {
            return ShoppingCart;
        }

        public Dictionary<Guid, int> GetShoppingCart()
        {
            return ShoppingCart;
        }

        public void AddToCart(Guid productId, int? quantity)
        {
            // If quantity is null or less than 1, default to 1
            int validatedQuantity = quantity.HasValue && quantity.Value > 0 ? quantity.Value : 1;

            if (ShoppingCart.ContainsKey(productId))
            {
                ShoppingCart[productId] += validatedQuantity;
            }
            else
            {
                ShoppingCart[productId] = validatedQuantity;
            }
        }

        public void RemoveItemFromCart(Guid productId)
        {
            if (ShoppingCart.ContainsKey(productId))
            {
                ShoppingCart.Remove(productId);
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found in the shopping cart.");
            }
        }

        public void RemoveItemQuantityFromCart
            (Guid productId, int? quantity)
        {
            if (!ShoppingCart.ContainsKey(productId))
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found in the shopping cart.");
            }

            // If quantity is null or less than 1, default to 1
            int validatedQuantity = quantity.HasValue && quantity.Value > 0 ? quantity.Value : 1;
            ShoppingCart[productId] -= validatedQuantity;

            // If the quantity drops to 0 or below, remove the item from the cart
            if (ShoppingCart[productId] <= 0)
            {
                ShoppingCart.Remove(productId);
            }
        }

        public void ClearCart()
        {
            ShoppingCart.Clear();
        }

    }
}
