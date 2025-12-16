using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTend_ProductAndShoppingCart.Data.DataModels
{
    public record Product(
       Guid Id,
       string Name,
       decimal Price,
       string? Description,
       int UnitsInStock);
}
