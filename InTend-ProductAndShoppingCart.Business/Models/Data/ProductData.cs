using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTend_ProductAndShoppingCart.Business.Models.Data
{
    public record ProductData(
       Guid Id,
       string Name,
       decimal Price,
       string? Description,
       int UnitsInStock);
}
