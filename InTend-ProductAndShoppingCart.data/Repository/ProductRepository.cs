using InTend_ProductAndShoppingCart.Business.Exceptions;
using InTend_ProductAndShoppingCart.Business.Models.Data;
using InTend_ProductAndShoppingCart.Business.Repository;

namespace InTend_ProductAndShoppingCart.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository() { }

        private readonly Dictionary<Guid, ProductData> Products = new()
        {
            [Guid.Parse("9eddf9df-04d5-47e2-aef7-bb99853c616d")] = new ProductData(
                Guid.Parse("9eddf9df-04d5-47e2-aef7-bb99853c616d"),
                "Laptop",
                999.99m,
                "A high-performance laptop suitable for all your computing needs.",
                7
            ),
            [Guid.Parse("48d57da5-ec8d-463c-a25a-12f989d68c8c")] = new ProductData(
                Guid.Parse("48d57da5-ec8d-463c-a25a-12f989d68c8c"),
                "Smartphone",
                699.99m,
                "A sleek smartphone with the latest features and a stunning display.",
                18
            ),
            [Guid.Parse("fef95e43-dc8d-4d4f-81d3-cd3877619d3b")] = new ProductData(
                Guid.Parse("fef95e43-dc8d-4d4f-81d3-cd3877619d3b"),
                "Headphones",
                199.99m,
                "Noise-cancelling headphones for an immersive audio experience.",
                124
            ),
            [Guid.Parse("fa3802b3-fd00-4440-a076-e62670198ceb")] = new ProductData(
                Guid.Parse("fa3802b3-fd00-4440-a076-e62670198ceb"),
                "Banana",
                1m,
                "The Golden Banana, just one pound",
                1000
            ),
            [Guid.Parse("f9739e25-4642-48d1-ab08-fa001dca79a5")] = new ProductData(
                Guid.Parse("f9739e25-4642-48d1-ab08-fa001dca79a5"),
                "24-Carat Gold Labubu",
                10000.00m,
                "The Golden Labubu",
                1
            ),
            [Guid.Parse("11111111-1111-1111-1111-111111111111")] = new ProductData(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "TestItemOne",
                10.00m,
                "TestItemOne",
                10
            ),
            [Guid.Parse("22222222-2222-2222-2222-222222222222")] = new ProductData(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "TestItemTwo",
                20.00m,
                "TestItemTwo",
                5
            )
        };

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
