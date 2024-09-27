using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class MaxRangeBadRequestException : BadRequestException
    {
        public MaxRangeBadRequestException()
         : base("Max value can't be less than min value.\"")
        {
        }
    }
}
