using FluentAssertions;
using InTend_ProductAndShoppingCart.Business.Api;
using InTend_ProductAndShoppingCart.Business.Exceptions;
using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;
using InTend_ProductAndShoppingCart.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace InTend_ProductAndShoppingCart.Business.Test.Api
{
    [TestClass]
    public class ProductApiTests
    {
        private IShoppingCartRepository _shoppingCartRepo = null!;
        private IProductRepository _productRepo = null!;
        private IReadOnlyDictionary<Guid, Product> _productLookup = null!;
        private ProductApi _productApi = null!;
        private ShoppingCartApi _shoppingCartApi = null!;

        private Guid _testGuidOne = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private Guid _testGuidTwo = Guid.Parse("22222222-2222-2222-2222-222222222222");

        public ProductApiTests()
        {
            // Parameterless constructor required by MSTest
        }

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();

            var provider = services.BuildServiceProvider();

            _productRepo = provider.GetRequiredService<IProductRepository>();
            _shoppingCartRepo = provider.GetRequiredService<IShoppingCartRepository>();
            _productApi = new ProductApi(_productRepo);
            _shoppingCartApi = new ShoppingCartApi(_shoppingCartRepo, _productApi);
            _productLookup = _productApi.GetAll();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _shoppingCartApi.ClearCart();
        }

        [TestMethod]
        public void GetProductById_GivenValidId_ReturnsCorrectProduct()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var expectedProduct = _productLookup[_testGuidOne];

            // Act
            var retrievedProduct = productApi.GetById(_testGuidOne);

            // Assert
            retrievedProduct.Should().BeEquivalentTo(expectedProduct);
        }

        [TestMethod]
        public void GetProductById_GivenInvalidNotFountId_ThrowsKeyNotFoundException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);

            // Act
            Action act = () => productApi.GetById(Guid.NewGuid());

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }


        [TestMethod]
        public void GetProductById_GivenEmptyGuid_ThrowsArgumentException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);

            // Act
            Action act = () => productApi.GetById(Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetProductStockQuantity_GivenValidId_ReturnsCorrectStockQuantity()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var expectedStockQuantity = 10;

            // Act
            var stockQuantity = productApi.GetProductStockQuantity(_testGuidOne);

            // Assert
            stockQuantity.Should().Be(expectedStockQuantity);
        }

        [TestMethod]
        public void GetProductStockQuantity_GivenInvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            // Act
            Action act = () => productApi.GetProductStockQuantity(Guid.NewGuid());
            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void IncreaseProductStock_GivenValidId_IncreasesStockSuccessfully()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var initialStock = productApi.GetProductStockQuantity(_testGuidOne);
            var stockToAdd = 5;

            // Act
            productApi.IncreaseProductStock(_testGuidOne, stockToAdd);
            var updatedStock = productApi.GetProductStockQuantity(_testGuidOne);

            // Assert
            updatedStock.Should().Be(initialStock + stockToAdd);
        }

        [TestMethod]
        public void IncreaseProductStock_GivenInvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var invalidProductId = Guid.NewGuid();
            // Act
            Action act = () => productApi.IncreaseProductStock(invalidProductId, 5);
            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void DecreaseProductStock_GivenValidId_DecreasesStockSuccessfully()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var initialStock = productApi.GetProductStockQuantity(_testGuidTwo);
            var stockToRemove = 2;
            // Act
            productApi.DecreaseProductStock(_testGuidTwo, stockToRemove);
            var updatedStock = productApi.GetProductStockQuantity(_testGuidTwo);
            // Assert
            updatedStock.Should().Be(initialStock - stockToRemove);
        }

        [TestMethod]
        public void DecreaseProductStock_GivenInvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var invalidProductId = Guid.NewGuid();
            // Act
            Action act = () => productApi.DecreaseProductStock(invalidProductId, 3);
            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void DecreaseProductStock_GivenInsufficientStock_ThrowsItemOutOfStockException()
        {
            // Arrange
            ProductApi productApi = new(_productRepo);
            var initialStock = productApi.GetProductStockQuantity(_testGuidTwo);
            var stockToRemove = initialStock + 10; // More than available stock
            // Act
            Action act = () => productApi.DecreaseProductStock(_testGuidTwo, stockToRemove);
            // Assert
            act.Should().Throw<ItemOutOfStockException>();
        }
    }
}
