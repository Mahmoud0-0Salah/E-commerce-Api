using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int OrderId)
        : base($"The Order with id: {OrderId} doesn't exist in the database.")
        {
        }
    }
}
