using InTend_ProductAndShoppingCart.Business.Exceptions;
using InTend_ProductAndShoppingCart.Business.Models.Data;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        // Singleton instance
        private static readonly Lazy<ProductRepository> _instance = new(() => new ProductRepository());

        // Public accessor for the singleton instance
        public static ProductRepository Instance => _instance.Value;

        public ProductRepository() { }

        private readonly Dictionary<Guid, ProductData> Products = new();

        public void PopulateProducts()
        {
            var product1 = new ProductData(Guid.Parse("9eddf9df-04d5-47e2-aef7-bb99853c616d"), "Laptop", 999.99m, "A high-performance laptop suitable for all your computing needs.", 7);
            var product2 = new ProductData(Guid.Parse("48d57da5-ec8d-463c-a25a-12f989d68c8c"), "Smartphone", 699.99m, "A sleek smartphone with the latest features and a stunning display.", 18);
            var product3 = new ProductData(Guid.Parse("fef95e43-dc8d-4d4f-81d3-cd3877619d3b"), "Headphones", 199.99m, "Noise-cancelling headphones for an immersive audio experience.", 124);
            var product4 = new ProductData(Guid.Parse("11111111-1111-1111-1111-111111111111"), "TestItemOne", 10.00m, "TestItemOne", 10);
            var product5 = new ProductData(Guid.Parse("22222222-2222-2222-2222-222222222222"), "TestItemTwo", 20.00m, "TestItemTwo", 5);

            Products[product1.Id] = product1;
            Products[product2.Id] = product2;
            Products[product3.Id] = product3;
            Products[product4.Id] = product4;
            Products[product5.Id] = product5;
        }

        public IReadOnlyDictionary<Guid, ProductData> GetAllProducts()
        {
            return Products;
        }

        public ProductData GetProductById(Guid productId)
        {
            if (Products.TryGetValue(productId, out var product))
            {
                return product;
            }
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        public int GetProductStock(Guid productId)
        {
            if (Products.TryGetValue(productId, out var product))
            {
                return product.UnitsInStock;
            }
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        public void InreaseProductStock(Guid productId, int stockToAdd)
        {
            if (Products.TryGetValue(productId, out var product))
            {
                var updatedProduct = product with { UnitsInStock = product.UnitsInStock + stockToAdd };
                Products[productId] = updatedProduct;
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
        }

        public void DecreaseProductStock(Guid productId, int stockToRemove)
        {
            if (Products.TryGetValue(productId, out var product))
            {
                if (product.UnitsInStock - stockToRemove < 0)
                {
                    throw new ItemOutOfStockException(productId, product.UnitsInStock, stockToRemove);
                }
                var updatedProduct = product with { UnitsInStock = product.UnitsInStock - stockToRemove };
                Products[productId] = updatedProduct;
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
        }
    }
}
