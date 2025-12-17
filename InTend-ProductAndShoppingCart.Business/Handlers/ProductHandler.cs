using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ProductHandler
    {
        private readonly IProductRepository _productRepository;

        internal ProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        internal void increaseProductStock(Guid productId, int stockToAdd)
        {
            _productRepository.InreaseProductStock(productId, stockToAdd);
        }

        internal void DecreaseProductStock(Guid productId, int stockToRemove)
        {
            _productRepository.DecreaseProductStock(productId, stockToRemove);
        }
    }
}
