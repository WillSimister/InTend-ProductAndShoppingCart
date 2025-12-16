using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Business.Models;
using InTend_ProductAndShoppingCart.Repository;

namespace InTend_ProductAndShoppingCart.Business.Api
{
    public class ProductApi
    {
        private readonly ProductRetriever _productRetriever;

        public ProductApi(ProductRepository productRepository)
        {
            _productRetriever = new ProductRetriever(productRepository);
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
    }
}
