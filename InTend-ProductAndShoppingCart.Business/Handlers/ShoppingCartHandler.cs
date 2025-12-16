using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ShoppingCartHandler
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        internal ShoppingCartHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        internal void AddToCart(Guid productId, int? quantity)
        {   
            _shoppingCartRepository.AddToCart(productId, quantity);
        }

        internal void RemoveItemFromCart(Guid productId)
        {
            _shoppingCartRepository.RemoveItemFromCart(productId);
        }

        internal void RemoveItemQuantityFromCart(Guid productId, int quantity)
        {
            _shoppingCartRepository.RemoveItemQuantityFromCart(productId, quantity);
        }

        internal void ClearCart()
        {
            _shoppingCartRepository.ClearCart();
        }
    }
}
