namespace InTend_ProductAndShoppingCart.Business.Validation
{
    internal static class ProductInputValidator
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 5000;

        internal static void ValidateNewProduct(string name, decimal price, string description)
        {
            ValidateName(name);
            ValidatePrice(price);
            ValidateDescription(description);
        }

        internal static void ValidateUpdateProduct(Guid productId, string? name = null, decimal? price = null, string? description = null)
        {
            // I thought about checking if the product exists here by calling productRetriever.GetProductById(productId)
            // however that would introduce an unncessary call to the db here, as when we try to update the product later on,
            // we will again try to get the product by id to perform the update, so I will just validate the id format here

            // Not sure what the best approach is, irl I would discuss this with the team

            ValidateId(productId);
            if (name != null)
            {
                ValidateName(name);
            }
            if (price != null)
            {
                ValidatePrice(price.Value);
            }
            if (description != null)
            {
                ValidateDescription(description);
            }
        }

        internal static void ValidateId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be an empty GUID.");
            }
        }

        internal static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.");
            }
            if (name.Length > MaxNameLength)
            {
                throw new ArgumentException($"Product name cannot exceed {MaxNameLength} characters.");
            }
        }

        internal static void ValidatePrice(decimal price)
        {
            //Assumption made that price can be zero but not negative
            if (price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }
            //A price cannot have more than 2 decimal places
            if (decimal.Round(price, 2) != price)
            {
                throw new ArgumentException("Product price cannot have more than 2 decimal places.");
            }
        }

        internal static void ValidateDescription(string description)
        {
            if (description.Length > MaxDescriptionLength)
            {
                throw new ArgumentException($"Product description cannot exceed {MaxDescriptionLength} characters.");
            }
        }
    }
}
