using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Repository;
using InTend_ProductAndShoppingCart.Business.Models;

namespace InTend_ProductAndShoppingCart.Business.Api
{
    public class ProductApi
    {
        private readonly ProductHandler _productHandler;
        private readonly ProductRetriever _productRetriever;

        public ProductApi(ProductRepository productRepository)
        {
            _productHandler = new ProductHandler(productRepository);
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

        public Product CreateProduct(string name, decimal price, string description)
        {
            return _productHandler.CreateProduct(name, price, description);
        }

        public Product UpdateName(Guid productGuid, string name)
        {
            return _productHandler.UpdateName(productGuid, name);
        }

        public Product UpdatePrice(Guid productGuid, decimal price)
        {
            return _productHandler.UpdatePrice(productGuid, price);
        }

        public Product UpdateDescription(Guid productId, string description)
        {
            return _productHandler.UpdateDescription(productId, description);
        }

        public void DeleteProduct(Guid productId)
        {
            _productHandler.DeleteProduct(productId);
        }
    }
}
