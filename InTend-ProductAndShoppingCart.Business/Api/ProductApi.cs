using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Business.Models;
using InTend_ProductAndShoppingCart.Repository;

namespace InTend_ProductAndShoppingCart.Business.Api
{
    public class ProductApi
    {
        private readonly ProductRetriever _productRetriever;
        private readonly ProductHandler _productHandler;

        public ProductApi(ProductRepository productRepository)
        {
            _productRetriever = new ProductRetriever(productRepository);
            _productHandler = new ProductHandler(productRepository);
        }

        public IReadOnlyDictionary<Guid, Product> GetAll()
        {
            return _productRetriever.GetAllProducts();
        }

        public Product GetById(Guid productId)
        {
            Product? product = _productRetriever.GetProductById(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            return product;
        }

        internal int GetProductStockQuantity(Guid productId)
        {
            return _productRetriever.GetProductStock(productId);
        }

        internal void IncreaseProductStock(Guid productId, int stockToAdd)
        {
            _productHandler.increaseProductStock(productId, stockToAdd);
        }

        internal void DecreaseProductStock(Guid productId, int stockToRemove)
        {
            _productHandler.DecreaseProductStock(productId, stockToRemove);
        }
    }
}
