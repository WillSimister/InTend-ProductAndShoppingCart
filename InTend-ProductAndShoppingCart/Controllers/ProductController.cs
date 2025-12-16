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
        return Ok(_productApi.UpdateName(productGuid, name));
    }

    [HttpPut("{productGuid}/price", Name = "UpdatePrice")]
    public IActionResult UpdatePrice(Guid productGuid, [FromQuery] decimal price)
    {
        return Ok(_productApi.UpdatePrice(productGuid, price));
    }

    [HttpPut("{productId}/description", Name = "UpdateDescription")]
    public IActionResult UpdateDescription(Guid productId, [FromQuery] string description)
    {
        return Ok(_productApi.UpdateDescription(productId, description));
    }

    [HttpDelete("{productId}", Name = "DeleteProduct")]
    public IActionResult DeleteProduct(Guid productId)
    {
        _productApi.DeleteProduct(productId);
        return Ok();
    }

}
