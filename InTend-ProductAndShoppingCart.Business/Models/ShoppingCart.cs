using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTend_ProductAndShoppingCart.Business.Models
{
    public record ShoppingCart (
        Dictionary<Product, int> shoppingCartItems,
        int totalProducts,
        decimal totalPrice);
}
