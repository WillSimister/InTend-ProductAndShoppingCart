using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Data.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public ShoppingCartRepository()
        {
        }

        private readonly Dictionary<Guid, int> shoppingCart = new();

        public IReadOnlyList<ShoppingCartItem> GetCartContents(IReadOnlyDictionary<Guid, Product> _productLookup)
        {
            var shoppingCartItems = new List<ShoppingCartItem>();

            foreach (var item in shoppingCart)
            {
                if (_productLookup.TryGetValue(item.Key, out var product))
                {
                    shoppingCartItems.Add(new ShoppingCartItem(product, item.Value));
                }
            }

            return shoppingCartItems;
        }

        public Dictionary<Guid, int> GetShoppingCart()
        {
            return shoppingCart;
        }

        public decimal GetCartTotal(IReadOnlyDictionary<Guid, Product> productLookup)
        {
            decimal total = 0m;
            foreach (var item in shoppingCart)
            {
                Guid productId = item.Key;
                int quantity = item.Value;
                decimal price = productLookup.ContainsKey(productId) ? productLookup[productId].Price : 0m;
                total += price * quantity;
            }
            return total;
        }

        public int GetQuantityOfItemInCart(Guid productId)
        {
            return shoppingCart.ContainsKey(productId) ? shoppingCart[productId] : 0;
        }

        public int GetTotalProductsInCart()
        {
            int totalItems = 0;
            foreach (var quantity in shoppingCart.Values)
            {
                totalItems += quantity;
            }
            return totalItems;
        }

        public void AddToCart(Guid productId, int? quantity)
        {
            // If quantity is null or less than 1, default to 1
            int validatedQuantity = quantity.HasValue && quantity.Value > 0 ? quantity.Value : 1;

            if (shoppingCart.ContainsKey(productId))
            {
                shoppingCart[productId] += validatedQuantity;
            }
            else
            {
                shoppingCart[productId] = validatedQuantity;
            }
        }

        public void RemoveItemFromCart(Guid productId)
        {
            if (shoppingCart.ContainsKey(productId))
            {
                shoppingCart.Remove(productId);
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found in the shopping cart.");
            }
        }

        public void RemoveItemQuantityFromCart
            (Guid productId, int? quantity)
        {
            if (!shoppingCart.ContainsKey(productId))
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found in the shopping cart.");
            }

            // If quantity is null or less than 1, default to 1
            int validatedQuantity = quantity.HasValue && quantity.Value > 0 ? quantity.Value : 1;
            shoppingCart[productId] -= validatedQuantity;

            // If the quantity drops to 0 or below, remove the item from the cart
            if (shoppingCart[productId] <= 0)
            {
                shoppingCart.Remove(productId);
            }
        }

        public void ClearCart()
        {
            shoppingCart.Clear();
        }
    }
}
