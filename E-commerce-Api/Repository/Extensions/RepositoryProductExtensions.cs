using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryProductExtensions
    {

        public static IQueryable<Product> Paging(this IQueryable<Product> Products, int PageNumber, int PageSize)
        {
            return Products
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize);
        }
        public static IQueryable<Product> Filter(this IQueryable<Product> Products, uint minprice, uint maxprice) => Products.Where(e => (e.Price >= minprice && e.Price <= maxprice));

        public static IQueryable<Product> Search(this IQueryable<Product> Products, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Products;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return Products.Where(e => e.Name.ToLower().Contains(lowerCaseTerm) || e.Description.ToLower().Contains(lowerCaseTerm) );
        }

        public static IQueryable<Product> Sort(this IQueryable<Product> product, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return product;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Product>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return product;

            return product.OrderBy(orderQuery);
        }


    }
}
