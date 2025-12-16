using InTend_ProductAndShoppingCart.Business.Api;
using InTend_ProductAndShoppingCart.data.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ShoppingCartHandler
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;

        internal ShoppingCartHandler(
            ShoppingCartRepository shoppingCartRepository)
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
    }
}
