using InTend_ProductAndShoppingCart.Business.Models.Data;

namespace InTend_ProductAndShoppingCart.Business.Repository
{
    public interface IProductRepository
    {
        public IReadOnlyDictionary<Guid, ProductData> GetAllProducts();
        public ProductData GetProductById(Guid productId);
        public int GetProductStock(Guid productId);
        public void InreaseProductStock(Guid productId, int stockToAdd);
        public void DecreaseProductStock(Guid productId, int stockToRemove);
    }
}
