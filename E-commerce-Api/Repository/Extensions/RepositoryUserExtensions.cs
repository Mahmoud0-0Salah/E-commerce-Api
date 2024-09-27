using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Repository.Extensions
{
    public static class RepositoryUserExtensions
    {
        public static IQueryable<User> Paging(this IQueryable<User> Users, int PageNumber, int PageSize)
        {
            return Users
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize);
        }

        public static IQueryable<User> Search(this IQueryable<User> Users, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Users;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return Users.Where(e => (e.FirstName.ToLower().Contains(lowerCaseTerm)) || (e.LastName.ToLower().Contains(lowerCaseTerm))  );
        }

    }
}
