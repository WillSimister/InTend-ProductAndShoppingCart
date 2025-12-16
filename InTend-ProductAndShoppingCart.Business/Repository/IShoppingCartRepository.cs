namespace InTend_ProductAndShoppingCart.Business.Repository
{
    public interface IShoppingCartRepository
    {
        IReadOnlyDictionary<Guid, int> GetCartContents();
        void AddToCart(Guid productId, int? quantity);
        void RemoveItemFromCart(Guid productId);
        void RemoveItemQuantityFromCart(Guid productId, int? quantity);
        void ClearCart();
    }
}
