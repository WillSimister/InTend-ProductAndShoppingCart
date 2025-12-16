using InTend_ProductAndShoppingCart.Data.DataModels;

namespace InTend_ProductAndShoppingCart.data.DataModels
{
    internal record ShoppingCart(
        //Product with Quantity
        Dictionary<Product, int> shoppingCartItems);
}
