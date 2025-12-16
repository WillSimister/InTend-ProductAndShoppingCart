using InTend_ProductAndShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTend_ProductAndShoppingCart.Business.Handlers
{
    internal class ProductHandler
    {
        private readonly ProductRepository _productRepository;

        internal ProductHandler(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void increaseProductStock(Guid productId, int stockToAdd)
        {
            _productRepository.InreaseProductStock(productId, stockToAdd);
        }

        public void DecreaseProductStock(Guid productId, int stockToRemove)
        {
            _productRepository.DecreaseProductStock(productId, stockToRemove);
        }
    }
}
