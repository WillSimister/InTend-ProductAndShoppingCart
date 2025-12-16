#nullable enable

namespace InTend_ProductAndShoppingCart.Business.Models
{
    public record Product(
            Guid Id,
            string Name,
            decimal Price,
            string? Description,
            bool IsAvailable
        )
    {
        public static Product FromDataModel(Data.DataModels.Product dataModel)
        {
            return new Product(
                dataModel.Id,
                dataModel.Name,
                dataModel.Price,
                dataModel.Description,
                dataModel.UnitsInStock > 0
            );
        }
    }
}


