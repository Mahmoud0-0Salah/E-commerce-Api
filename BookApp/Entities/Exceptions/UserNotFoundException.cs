using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string CateogryId)
        : base($"The User with id: {CateogryId} doesn't exist in the database.")
        {
        }
    }
}
