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
            var cartContents = _shoppingCartRepository.GetCartContents();

            var shoppingCartItems = new List<ShoppingCartItem>();

            foreach (var item in cartContents)
            {
                if (_productLookup.TryGetValue(item.Key, out var product))
                {
                    shoppingCartItems.Add(new ShoppingCartItem(product, item.Value, product.Price * item.Value));
                }
            }

            int totalProducts = shoppingCartItems.Sum(i => i.Quantity);
            decimal totalPrice = shoppingCartItems.Sum(i => i.Product.Price * i.Quantity);

            return new ShoppingCart(shoppingCartItems, totalProducts, totalPrice);
        }

        internal int GetQuantityOfItemInCart(Guid productGuid)
        {
            var cartContents = _shoppingCartRepository.GetCartContents();
            return cartContents.TryGetValue(productGuid, out var quantity) ? quantity : 0;
        }

        internal IReadOnlyDictionary<Guid, int> GetCartContents()
        {
            return _shoppingCartRepository.GetCartContents();
        }
    }
}
