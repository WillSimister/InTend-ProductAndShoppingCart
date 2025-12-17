namespace InTend_ProductAndShoppingCart.Business.Models.Data
{
    internal record ShoppingCartData(
        //Product with Quantity
        Dictionary<ProductData, int> shoppingCartItems);
}
