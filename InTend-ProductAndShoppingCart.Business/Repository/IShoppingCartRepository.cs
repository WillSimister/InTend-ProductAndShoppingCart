using InTend_ProductAndShoppingCart.Business.Models.Business;

namespace InTend_ProductAndShoppingCart.Business.Repository
{
    public interface IShoppingCartRepository
    {
        IReadOnlyList<ShoppingCartItem> GetCartContents(IReadOnlyDictionary<Guid, Product> _productLookup);
        int GetTotalProductsInCart();
        int GetQuantityOfItemInCart(Guid productId);
        decimal GetCartTotal(IReadOnlyDictionary<Guid, Product> productLookup);
        void AddToCart(Guid productId, int? quantity);
        void RemoveItemFromCart(Guid productId);
        void RemoveItemQuantityFromCart(Guid productId, int? quantity);
        void ClearCart();
    }
}
