namespace InTend_ProductAndShoppingCart.Business.Models.Business
{
    public record ShoppingCart (
        Dictionary<Product, int> Items,
        int TotalProducts,
        decimal TotalPrice);
}
