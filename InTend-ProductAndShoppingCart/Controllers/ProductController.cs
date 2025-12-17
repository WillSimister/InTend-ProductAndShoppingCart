using InTend_ProductAndShoppingCart.Business.Models.Business;
using InTend_ProductAndShoppingCart.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InTend_ProductAndShoppingCart.Controllers;

[ApiController]
[Route("[controller]/[Action]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly Business.Api.ProductApi _productApi;

    public ProductController(
        ILogger<ProductController> logger)
    {
        _logger = logger;
        _productApi = new Business.Api.ProductApi(ProductRepository.Instance);
    }

    [HttpGet(Name = "GetAll")]
    public IReadOnlyDictionary<Guid, Product> GetAll()
    {
        return _productApi.GetAll();
    }

    [HttpGet("{productId}", Name = "GetById")]
    public IActionResult GetById(Guid productId)
    {
        try
        {
            var product = _productApi.GetById(productId);
            return Ok(product);
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
