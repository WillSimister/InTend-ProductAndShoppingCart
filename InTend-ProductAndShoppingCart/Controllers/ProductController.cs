using InTend_ProductAndShoppingCart.Repository;
using Microsoft.AspNetCore.Mvc;
using InTend_ProductAndShoppingCart.Business.Models;

namespace InTend_ProductAndShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
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
    public Product GetById(Guid productId)
    {
        return _productApi.GetById(productId);
    }

    [HttpPost(Name = "CreateProduct")]
    public IActionResult CreateProduct([FromQuery] string name, [FromQuery] decimal price, [FromQuery] string description)
    {
        return Ok(_productApi.CreateProduct(name, price, description));
    }

    [HttpPut("{productGuid}/name", Name = "UpdateName")]
    public IActionResult UpdateName(Guid productGuid, [FromQuery] string name)
    {
        try
        {
            return Ok(_productApi.UpdateName(productGuid, name));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{productGuid}/price", Name = "UpdatePrice")]
    public IActionResult UpdatePrice(Guid productGuid, [FromQuery] decimal price)
    {
        try
        {
            return Ok(_productApi.UpdatePrice(productGuid, price));
        }
        catch (KeyNotFoundException)
        {
            // See below Controller Endpoint for discussion on NotFound with messages
            return NotFound();
        }
    }

    [HttpPut("{productId}/description", Name = "UpdateDescription")]
    public IActionResult UpdateDescription(Guid productId, [FromQuery] string description)
    {
        try
        {
            return Ok(_productApi.UpdateDescription(productId, description));
        }
        catch (KeyNotFoundException)
        {
            // See below Controller Endpoint for discussion on NotFound with message
            return NotFound();
        }
    }

    [HttpDelete("{productId}", Name = "DeleteProduct")]
    public IActionResult DeleteProduct(Guid productId)
    {
        try
        {
            _productApi.DeleteProduct(productId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            // I want to caveat this final call to NotFound with a message, I have read articles arguing
            // for and against doing this, but I think it's more user friendly to provide some context.
            // however it does potentially leak information about the existence of resources, so in a real world scenario
            // I would discuss this with the team to decide on the best approach
            return NotFound(new
            {
                error = $"Product with ID '{productId}' not found."
            });
        }
    }
}
