using InTend_ProductAndShoppingCart.Repository;
using Microsoft.AspNetCore.Mvc;
using InTend_ProductAndShoppingCart.Business.Models;
using InTend_ProductAndShoppingCart.data.Repository;

namespace InTend_ProductAndShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly ILogger<ShoppingCartController> _logger;
    private readonly Business.Api.ProductApi _productApi;
    private readonly Business.Api.ShoppingCartApi _shoppingCartApi;

    public ShoppingCartController(
        ILogger<ShoppingCartController> logger)
    {
        _logger = logger;
        _productApi = new Business.Api.ProductApi(ProductRepository.Instance);
        _shoppingCartApi = new Business.Api.ShoppingCartApi(ShoppingCartRepository.Instance, _productApi);
    }

    [HttpGet(Name = "GetShoppingCart")]
    public ShoppingCart GetShoppingCart()
    {
        return _shoppingCartApi.GetShoppingCart();
    }

    [HttpPost(Name = "AddItemToCart")]
    public IActionResult AddItemToCart([FromQuery] Guid productId, [FromQuery] int? quantity)
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

    [HttpPut("{productId}", Name = "RemoveItemQuantityFromCart")]
    public IActionResult RemoveItemQuantityFromCart(Guid productId, [FromQuery] int quantity)
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
}
