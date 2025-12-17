using InTend_ProductAndShoppingCart.Business.Handlers;
using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Api
{
    public class ProductApi
    {
        private readonly ProductRetriever _productRetriever;
        private readonly ProductHandler _productHandler;

        public ProductApi(IProductRepository productRepository)
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
            Validation.ProductInputValidator.ValidateId(productId);

            return _productRetriever.GetProductById(productId);
        }

        public int GetProductStockQuantity(Guid productId)
        {
            return _productRetriever.GetProductStock(productId);
        }

        public void IncreaseProductStock(Guid productId, int stockToAdd)
        {
            Validation.ProductInputValidator.ValidateId(productId);
            Validation.ProductInputValidator.ValidateQuantity(stockToAdd);

            _productHandler.increaseProductStock(productId, stockToAdd);
        }

        public void DecreaseProductStock(Guid productId, int stockToRemove)
        {
            Validation.ProductInputValidator.ValidateId(productId);
            Validation.ProductInputValidator.ValidateQuantity(stockToRemove);

            _productHandler.DecreaseProductStock(productId, stockToRemove);
        }
    }
}
