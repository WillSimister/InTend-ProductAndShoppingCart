namespace InTend_ProductAndShoppingCart.Business.Models.Data
{
    public record ProductData(
       Guid Id,
       string Name,
       decimal Price,
       string? Description,
       int UnitsInStock);
}
