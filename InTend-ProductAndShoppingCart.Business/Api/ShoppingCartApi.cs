using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Api
{
    public class ShoppingCartApi
    {
        private readonly ShoppingCartHandler _shoppingCartHandler;
        private readonly ShoppingCartRetriever _shoppingCartRetriever;
        private readonly ProductApi _productApi;

        public ShoppingCartApi(IShoppingCartRepository shoppingCartRepository, ProductApi productApi) 
        {
            _shoppingCartHandler = new ShoppingCartHandler(shoppingCartRepository);
            _shoppingCartRetriever = new ShoppingCartRetriever(shoppingCartRepository, productApi.GetAll());
            _productApi = productApi;
        }

        public ShoppingCart GetShoppingCart()
        {
            return _shoppingCartRetriever.GetShoppingCart();
        }

        public void AddToCart(Guid productId, int? quantity)
        {
            Validation.ProductInputValidator.ValidateId(productId);

            if (quantity.HasValue)
                Validation.ProductInputValidator.ValidateQuantity(quantity.Value);

            int itemQuantityInStock = _productApi.GetProductStockQuantity(productId);

            bool itemHasEnoughStock = Validation.ShoppingCartValidation.ItemHasEnoughStock(
                productId,
                quantity ?? 1,
                itemQuantityInStock);

            if (itemHasEnoughStock)
            {
                _shoppingCartHandler.AddToCart(productId, quantity);
                _productApi.DecreaseProductStock(productId, quantity ?? 1);
            }
        }

        public void RemoveItemFromCart(Guid productId)
        {
            Validation.ProductInputValidator.ValidateId(productId);

            int quantityInCart = _shoppingCartRetriever.GetQuantityOfItemInCart(productId);

            _shoppingCartHandler.RemoveItemFromCart(productId);
            _productApi.IncreaseProductStock(productId, quantityInCart);
        }

        public void RemoveItemQuantityFromCart(Guid productId, int quantity)
        {
            Validation.ProductInputValidator.ValidateId(productId);
            Validation.ProductInputValidator.ValidateQuantity(quantity);

            int quantityInCart = _shoppingCartRetriever.GetQuantityOfItemInCart(productId);

            _shoppingCartHandler.RemoveItemQuantityFromCart(productId, quantity);
            _productApi.IncreaseProductStock(productId, quantity);
        }

        public void ClearCart()
        {
            var cartContents = _shoppingCartRetriever.GetCartContents();

            foreach (var item in cartContents)
            {
                _productApi.IncreaseProductStock(item.Key, item.Value);
            }
            _shoppingCartHandler.ClearCart();
        }
    }
}
