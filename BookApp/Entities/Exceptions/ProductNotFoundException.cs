using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class ProductNotFoundException :NotFoundException
    {
        public ProductNotFoundException(int ProductId)
        : base($"Product with id: {ProductId} doesn't exist in the database.")
        {
        }
    }
}
