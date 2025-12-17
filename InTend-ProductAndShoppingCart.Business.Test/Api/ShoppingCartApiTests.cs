using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;
using InTend_ProductAndShoppingCart.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using InTend_ProductAndShoppingCart.Business.Api;
using InTend_ProductAndShoppingCart.Business.Exceptions;


[TestClass]
public class ShoppingCartApiTests
{
    private IShoppingCartRepository _shoppingCartRepo = null!;
    private IProductRepository _productRepo = null!;
    private IReadOnlyDictionary<Guid, Product> _productLookup = null!;

    private Guid _testGuidOne = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private Guid _testGuidTwo = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();

        var provider = services.BuildServiceProvider();

        _productRepo = provider.GetRequiredService<IProductRepository>();
        _shoppingCartRepo = provider.GetRequiredService<IShoppingCartRepository>();

        _productRepo.PopulateProducts();
        _productLookup = new ProductApi(_productRepo).GetAll();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _shoppingCartRepo.ClearCart();
        _productRepo.PopulateProducts();
    }

    private static ShoppingCartItem? FindItemByProductId(IReadOnlyList<ShoppingCartItem> items, Guid productId)
        => items.FirstOrDefault(i => i.Product.Id == productId);

    [TestMethod]
    public void AddToCart_GivenValidProductIdAndQuantity_AddsToCartSuccessfully()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Act
        shoppingCartApi.AddToCart(_testGuidOne, 3);
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        var item = FindItemByProductId(cartContents.Items, _testGuidOne);
        item.Should().NotBeNull();
        item.Quantity.Should().Be(3);
    }

    [TestMethod]
    public void AddToCart_GivenInvalidProductId_ThrowsException()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.ClearCart();
        var invalidProductId = Guid.NewGuid();

        // Act
        Action act = () => shoppingCartApi.AddToCart(invalidProductId, 2);

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void AddToCart_GivenAProductWithLessStockThanRequestedQuantity_ThrowsItemOutOfStockException()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Act
        Action act = () => shoppingCartApi.AddToCart(_testGuidTwo, 10); // TestItemTwo has only 5 in stock

        // Assert
        act.Should().Throw<ItemOutOfStockException>()
            .WithMessage($"Product with ID '{_testGuidTwo}' does not have enough stock to complete request. Current Stock Level: {5}, Requested Stock: {10}");
    }

    [TestMethod]
    public void GetShoppingCart_GivenNoProductsInCart_ReturnsExpectedResults()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.ClearCart();

        // Act
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        cartContents.Items.Should().BeEmpty();
        cartContents.TotalProducts.Should().Be(0);
        cartContents.TotalPrice.Should().Be(0m);
    }

    [TestMethod]
    public void GetShoppingCart_GivenOneProductInCart_ReturnsWithCorrectTotals()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.ClearCart();

        // Act
        shoppingCartApi.AddToCart(_testGuidOne, 2);
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        cartContents.Items.Should().ContainSingle(i => i.Product.Id == _testGuidOne && i.Quantity == 2);
        cartContents.TotalProducts.Should().Be(2);
        cartContents.TotalPrice.Should().Be(_productLookup[_testGuidOne].Price * 2);
    }

    [TestMethod]
    public void GetShoppingCart_GivenTwoProductsInCart_ReturnsWithCorrectTotals()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.ClearCart();

        // Act
        shoppingCartApi.AddToCart(_testGuidOne, 2);
        shoppingCartApi.AddToCart(_testGuidTwo, 3);
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        cartContents.Items.Should().Contain(i => i.Product.Id == _testGuidOne && i.Quantity == 2);
        cartContents.Items.Should().Contain(i => i.Product.Id == _testGuidTwo && i.Quantity == 3);
        cartContents.TotalProducts.Should().Be(5);
        cartContents.TotalPrice.Should().Be((_productLookup[_testGuidOne].Price * 2) + (_productLookup[_testGuidTwo].Price * 3));
    }

    [TestMethod]
    public void ClearCart_AfterAddingProducts_CartIsEmpty()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 2);
        shoppingCartApi.AddToCart(_testGuidTwo, 3);

        // Act
        shoppingCartApi.ClearCart();
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        cartContents.Items.Should().BeEmpty();
        cartContents.TotalProducts.Should().Be(0);
        cartContents.TotalPrice.Should().Be(0m);
    }

    [TestMethod]
    public void RemoveItemFromCart_GivenValidProductId_RemovesItemSuccessfully()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));

        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 2);
        shoppingCartApi.AddToCart(_testGuidTwo, 3);

        // Act
        shoppingCartApi.RemoveItemFromCart(_testGuidOne);
        var cartContents = shoppingCartApi.GetShoppingCart();

        // Assert
        cartContents.Should().NotBeNull();
        cartContents.Items.Should().NotContain(i => i.Product.Id == _testGuidOne);
        cartContents.Items.Should().Contain(i => i.Product.Id == _testGuidTwo && i.Quantity == 3);
    }

    [TestMethod]
    public void RemoveItemFromCart_GivenInvalidProductId_ThrowsException()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));
        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 2);
        var invalidProductId = Guid.NewGuid();
        // Act
        Action act = () => shoppingCartApi.RemoveItemFromCart(invalidProductId);
        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void RemoveItemQuantityFromCart_GivenValidProductIdAndQuantity_RemovesQuantitySuccessfully()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));
        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 5);
        // Act
        shoppingCartApi.RemoveItemQuantityFromCart(_testGuidOne, 2);
        var cartContents = shoppingCartApi.GetShoppingCart();
        // Assert
        var item = FindItemByProductId(cartContents.Items, _testGuidOne);
        item.Should().NotBeNull();
        item.Quantity.Should().Be(3);
    }

    [TestMethod]
    public void RemoveItemQuantityFromCart_GivenInvalidProductId_ThrowsException()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));
        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 5);
        var invalidProductId = Guid.NewGuid();
        // Act
        Action act = () => shoppingCartApi.RemoveItemQuantityFromCart(invalidProductId, 2);
        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void RemoveItemQuantityFromCart_GivenQuantityGreaterThanInCart_RemovesItemCompletely()
    {
        ShoppingCartApi shoppingCartApi = new(_shoppingCartRepo, new ProductApi(_productRepo));
        // Arrange
        shoppingCartApi.AddToCart(_testGuidOne, 3);
        // Act
        shoppingCartApi.RemoveItemQuantityFromCart(_testGuidOne, 5); // Removing more than in cart
        var cartContents = shoppingCartApi.GetShoppingCart();
        // Assert
        cartContents.Items.Should().NotContain(i => i.Product.Id == _testGuidOne);
    }
}
