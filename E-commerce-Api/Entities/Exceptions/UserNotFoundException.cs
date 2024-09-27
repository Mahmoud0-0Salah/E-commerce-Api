using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string UserId)
        : base($"The User with id: {UserId} doesn't exist in the database.")
        {
        }
    }
}
