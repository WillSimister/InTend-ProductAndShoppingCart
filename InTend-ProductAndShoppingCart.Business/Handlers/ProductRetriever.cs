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

            var businessProducts = dataProducts.ToDictionary(
                kvp => kvp.Key,
                kvp => Models.Product.FromDataModel(kvp.Value)
            );
            return businessProducts;
        }

        public Models.Product? GetProductById(Guid productId)
        {
            var dataProduct = _productRepository.GetAllProducts()
                .Values
                .FirstOrDefault(p => p.Id == productId);

            return dataProduct == null
                ? null
                : Models.Product.FromDataModel(dataProduct);
        }
    }
}
