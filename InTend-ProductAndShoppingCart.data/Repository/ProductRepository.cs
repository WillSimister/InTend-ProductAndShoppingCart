using InTend_ProductAndShoppingCart.Data.DataModels;

namespace InTend_ProductAndShoppingCart.Repository
{
    public class ProductRepository
    {
        // Singleton instance
        private static readonly Lazy<ProductRepository> _instance = new(() => new ProductRepository());

        // Public accessor for the singleton instance
        public static ProductRepository Instance => _instance.Value;

        // Private constructor to prevent external instantiation
        private ProductRepository() { }

        private readonly Dictionary<Guid, Product> Products = new();

        public void PopulateProducts()
        {
            var product1 = new Product(Guid.Parse("9eddf9df-04d5-47e2-aef7-bb99853c616d"), "Laptop", 999.99m, "A high-performance laptop suitable for all your computing needs.");
            var product2 = new Product(Guid.Parse("48d57da5-ec8d-463c-a25a-12f989d68c8c"), "Smartphone", 699.99m, "A sleek smartphone with the latest features and a stunning display.");
            var product3 = new Product(Guid.Parse("fef95e43-dc8d-4d4f-81d3-cd3877619d3b"), "Headphones", 199.99m, "Noise-cancelling headphones for an immersive audio experience.");

            Products[product1.Id] = product1;
            Products[product2.Id] = product2;
            Products[product3.Id] = product3;
        }

        public IReadOnlyDictionary<Guid, Product> GetAllProducts()
        {
            return Products;
        }

        public Product? GetProductById(Guid productId)
        {
            Products.TryGetValue(productId, out var product);
            return product;
        }

        public Product AddProduct(string name, decimal price, string description)
        {
            var newProduct = new Product(Guid.NewGuid(), name, price, description);
            Products[newProduct.Id] = newProduct;
            return Products[newProduct.Id];
        }

        public Product UpdateName(Guid productGuid, string name)
        {
            if (Products.TryGetValue(productGuid, out var product))
            {
                var updatedProduct = product with { Name = name };
                Products[productGuid] = updatedProduct;
                return updatedProduct;
            }
            throw new KeyNotFoundException($"Product with ID {productGuid} not found.");
        }

        public Product UpdatePrice(Guid productGuid, decimal price)
        {
            if (Products.TryGetValue(productGuid, out var product))
            {
                var updatedProduct = product with { Price = price };
                Products[productGuid] = updatedProduct;
                return updatedProduct;
            }
            throw new KeyNotFoundException($"Product with ID {productGuid} not found.");
        }

        public Product UpdateDescription(Guid productGuid, string description)
        {
            if (Products.TryGetValue(productGuid, out var product))
            {
                var updatedProduct = product with { Description = description };
                Products[productGuid] = updatedProduct;
                return updatedProduct;
            }
            throw new KeyNotFoundException($"Product with ID {productGuid} not found.");
        }

        public void DeleteProduct(Guid productGuid)
        {
            if (!Products.Remove(productGuid))
            {
                throw new KeyNotFoundException($"Product with ID {productGuid} not found.");
            }
        }
    }
}
