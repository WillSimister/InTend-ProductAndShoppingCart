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

        public ShoppingCart GetShoppingCart()
        {
            var cartContents = _shoppingCartRepository.GetCartContents();

            var shoppingCartItems = new Dictionary<Product, int>();

            foreach (var item in cartContents)
            {
                if (_productLookup.TryGetValue(item.Key, out var product))
                {
                    shoppingCartItems[product] = item.Value;
                }
            }

            int totalProducts = shoppingCartItems.Values.Sum();
            decimal totalPrice = shoppingCartItems.Sum(item => item.Key.Price * item.Value);

            return new ShoppingCart(shoppingCartItems, totalProducts, totalPrice);
        }

        public int GetQuantityOfItemInCart(Guid productGuid)
        {
            var cartContents = _shoppingCartRepository.GetCartContents();
            return cartContents.TryGetValue(productGuid, out var quantity) ? quantity : 0;
        }

        public IReadOnlyDictionary<Guid, int> GetCartContents()
        {
            return _shoppingCartRepository.GetCartContents();
        }
    }
}
