using InTend_ProductAndShoppingCart.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ProductRetriever
    {
        private readonly ProductRepository _productRepository;

        internal ProductRetriever(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IReadOnlyDictionary<Guid, Models.Product> GetAllProducts()
        {
            var dataProducts = _productRepository.GetAllProducts();

            return dataProducts.ToDictionary(
                kvp => kvp.Key,
                kvp => Models.Product.FromDataModel(kvp.Value)
            );
        }

        public Models.Product GetProductById(Guid productId)
        {
            return Models.Product.FromDataModel(
                _productRepository
                .GetProductById(productId));
        }
    }
}
