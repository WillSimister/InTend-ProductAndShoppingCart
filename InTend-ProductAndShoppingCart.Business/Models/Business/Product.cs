#nullable enable

using InTend_ProductAndShoppingCart;
using InTend_ProductAndShoppingCart.Business.Models.Data;

namespace InTend_ProductAndShoppingCart.Business.Models.Business
{
    public record Product(
            Guid Id,
            string Name,
            decimal Price,
            string? Description,
            bool IsAvailable
        )
    {
        public static Product FromDataModel(ProductData dataModel)
        {
            return new Product(
                dataModel.Id,
                dataModel.Name,
                dataModel.Price,
                dataModel.Description,
                dataModel.UnitsInStock > 0 //Decided on this over showing actual stock level as this is how most e-commerce sites act
            );
        }
    }
}


