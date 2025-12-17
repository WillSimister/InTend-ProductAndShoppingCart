using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Business.Repository;
using InTend_ProductAndShoppingCart.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InTend_ProductAndShoppingCart.Controllers;

[ApiController]
[Route("[controller]/[Action]")]
public class ShoppingCartController : ControllerBase
{
    private readonly ILogger<ShoppingCartController> _logger;
    private readonly Business.Api.ProductApi _productApi;
    private readonly Business.Api.ShoppingCartApi _shoppingCartApi;

    public ShoppingCartController(
        ILogger<ShoppingCartController> logger,
        IShoppingCartRepository shoppingCartRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productApi = new Business.Api.ProductApi(productRepository);
        _shoppingCartApi = new Business.Api.ShoppingCartApi(shoppingCartRepository, _productApi);
    }

    [HttpGet(Name = "ShoppingCart")]
    public ShoppingCart ShoppingCart()
    {
        return _shoppingCartApi.GetShoppingCart();
    }

    [HttpPost("{productId}", Name = "AddItemToCart")]
    public IActionResult AddItemToCart(Guid productId, [FromQuery] int? quantity = null)
    {
        try
        {
            _shoppingCartApi.AddToCart(productId, quantity);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Product with ID '{productId}' not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", productId);
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }

    [HttpPut("{productId}/quantity/{quantity}", Name = "RemoveItemQuantityFromCart")]
    public IActionResult RemoveItemQuantityFromCart(Guid productId, int quantity)
    {
        try
        {
            _shoppingCartApi.RemoveItemQuantityFromCart(productId, quantity);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Product with ID '{productId}' not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", productId);
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }

    [HttpDelete("{productId}", Name = "RemoveItemFromCart")]
    public IActionResult RemoveItemFromCart(Guid productId)
    {
        try
        {
            _shoppingCartApi.RemoveItemFromCart(productId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Product with ID '{productId}' not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", productId);
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }

    [HttpDelete]
    public IActionResult ClearCart()
    {
        try
        {
            _shoppingCartApi.ClearCart();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing the shopping cart");
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }
}
