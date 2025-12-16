using InTend_ProductAndShoppingCart.Repository;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ProductHandler
    {
        private readonly ProductRepository _productRepository;

        internal ProductHandler(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        internal Models.Product CreateProduct(string name, decimal price, string description)
        {
            Data.DataModels.Product productDataModel = _productRepository.AddProduct(name, price, description);

            return Models.Product.FromDataModel(productDataModel);
        }

        internal Models.Product UpdateName(Guid productGuid, string name)
        {
            Data.DataModels.Product productDataModel = _productRepository.UpdateName(productGuid, name);

            return Models.Product.FromDataModel(productDataModel);
        }

        internal Models.Product UpdatePrice(Guid productGuid, decimal price)
        {
            Data.DataModels.Product productDataModel = _productRepository.UpdatePrice(productGuid, price);

            return Models.Product.FromDataModel(productDataModel);
        }

        internal Models.Product UpdateDescription(Guid productId, string description)
        {
            Data.DataModels.Product productDataModel = _productRepository.UpdateDescription(productId, description);

            return Models.Product.FromDataModel(productDataModel);
        }

        internal void DeleteProduct(Guid productId)
        {
            _productRepository.DeleteProduct(productId);
        }
    }
}
