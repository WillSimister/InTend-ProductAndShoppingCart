using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ProductRetriever
    {
        private readonly IProductRepository _productRepository;

        internal ProductRetriever(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IReadOnlyDictionary<Guid, Product> GetAllProducts()
        {
            var dataProducts = _productRepository.GetAllProducts();

            return dataProducts.ToDictionary(
                kvp => kvp.Key,
                kvp => Product.FromDataModel(kvp.Value)
            );
        }

        public Product GetProductById(Guid productId)
        {
            Validation.ProductInputValidator.ValidateId(productId);
            return Product.FromDataModel(
                _productRepository
                .GetProductById(productId));
        }

        public int GetProductStock(Guid productId)
        {
            Validation.ProductInputValidator.ValidateId(productId);
            return _productRepository.GetProductStock(productId);
        }
    }
}
