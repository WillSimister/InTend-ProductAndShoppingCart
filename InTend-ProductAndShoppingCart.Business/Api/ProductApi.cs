using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Repository;
using InTend_ProductAndShoppingCart.Business.Models;
using InTend_ProductAndShoppingCart.Business.Validation;

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
            ProductInputValidator.ValidateNewProduct(name, price, description);
            return _productHandler.CreateProduct(name, price, description);
        }

        public Product UpdateName(Guid productGuid, string name)
        {
            ProductInputValidator.ValidateUpdateProduct(productGuid, name: name);
            return _productHandler.UpdateName(productGuid, name);
        }

        public Product UpdatePrice(Guid productGuid, decimal price)
        {
            ProductInputValidator.ValidateUpdateProduct(productGuid, price: price);
            return _productHandler.UpdatePrice(productGuid, price);
        }

        public Product UpdateDescription(Guid productId, string description)
        {
            ProductInputValidator.ValidateUpdateProduct(productId, description: description);
            return _productHandler.UpdateDescription(productId, description);
        }

        public void DeleteProduct(Guid productId)
        {
            ProductInputValidator.ValidateId(productId); 
            _productHandler.DeleteProduct(productId);
        }
    }
}
