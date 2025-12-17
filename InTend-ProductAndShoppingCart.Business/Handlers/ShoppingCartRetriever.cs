using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ShoppingCartRetriever
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IReadOnlyDictionary<Guid, Product> _productLookup;

        internal ShoppingCartRetriever(IShoppingCartRepository shoppingCartRepository, IReadOnlyDictionary<Guid, Product> productLookup)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productLookup = productLookup;
        }

        internal ShoppingCart GetShoppingCart()
        {
            var cartContents = _shoppingCartRepository.GetCartContents(_productLookup);
            var totalProducts = _shoppingCartRepository.GetTotalProductsInCart();
            var totalPrice = _shoppingCartRepository.GetCartTotal(_productLookup);

            return new ShoppingCart(cartContents, totalProducts, totalPrice);
        }

        internal int GetQuantityOfItemInCart(Guid productGuid)
        {
            return _shoppingCartRepository.GetQuantityOfItemInCart(productGuid);
        }

        internal IReadOnlyDictionary<Guid, int> GetCartContents()
        {
            return _shoppingCartRepository.GetCartContents(_productLookup)
                .ToDictionary(item => item.Product.Id, item => item.Quantity);
        }
    }
}
