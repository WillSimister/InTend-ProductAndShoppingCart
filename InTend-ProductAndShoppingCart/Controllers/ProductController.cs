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
}
