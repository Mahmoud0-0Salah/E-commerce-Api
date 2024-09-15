using Repository.Extensions.Utility;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryCatgoryExtensions
    {

        public static IQueryable<Cateogry> Paging(this IQueryable<Cateogry> Cateogries,int PageNumber,int PageSize)
        {
            return Cateogries
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize);
        }

        public static IQueryable<Cateogry> Search(this IQueryable<Cateogry> Cateogries, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Cateogries;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return Cateogries.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Cateogry> Sort(this IQueryable<Cateogry> cateogry, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return cateogry;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Cateogry>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return cateogry;

            return cateogry.OrderBy(orderQuery);
        }
    }
}
