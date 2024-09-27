using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class NotEnoughAmountException : BadRequestException
    {
        public NotEnoughAmountException()
         : base("There is not enough amount")
        {
        }
    }
}
