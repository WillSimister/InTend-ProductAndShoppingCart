namespace InTend_ProductAndShoppingCart.Business.Models.Business
{
    public record ShoppingCartItem(
        Product Product,
        int Quantity
    );

    public record ShoppingCart(
        IReadOnlyList<ShoppingCartItem> Items,
        int TotalProducts,
        decimal TotalPrice
    );
}
